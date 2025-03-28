using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using StockSale.App.Data;
using StockSale.App.Models.Domain;
using StockSale.App.Models.ViewModels;
using StockSale.App.Repositories;
using StockSale.App.Repositories.Interfaces;
using StockSale.App.Services;
using System.Diagnostics;

namespace StockSale.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;
        private readonly ProviderService providerService;
        private readonly IUnitMRepository unitMRepository;
        private readonly PaginationService paginationService;
        private readonly ExcelService excelService;
        private readonly StockSaleDbContext stockSaleDbContext;

        public ProductController(ProductService productService,
                                 ProviderService providerService,
                                 IUnitMRepository unitMRepository,
                                 PaginationService paginationService,
                                 ExcelService excelService,
                                 StockSaleDbContext stockSaleDbContext)
        {
            this.productService = productService;
            this.providerService = providerService;
            this.unitMRepository = unitMRepository;
            this.paginationService = paginationService;
            this.excelService = excelService;
            this.stockSaleDbContext = stockSaleDbContext;
        }

        // Este método devuelve la vista con los proveedores y unidades de medida cargadas
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var providers = await providerService.GetAllAsync(); // Obtener proveedores
            var unitsM = await unitMRepository.GetAllAsync(); // Obtener unidades de medida

            // Crear un ViewModel para pasar los datos a la vista
            var viewModel = new AddProductViewModel
            {
                Providers = providers,
                UnitsM = unitsM
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            var lastAdded = await productService.LastAdded();
            int N = 1;
            if (lastAdded != null)
            {
                N = lastAdded.NProduct + 1;
            }
          
                var provider = await providerService.GetAsync(model.ProviderId.Value);
                var unitM = await unitMRepository.GetAsync(model.UnitMId.Value);
                var product = new Product
                {
                    NProduct = N,
                    Name = model.Name,
                    Packaging = model.Packaging,
                    Stock = model.Stock,
                    Stock_Limit = model.Stock_Limit,
                    List_Price = model.List_Price,
                    Sell_Price = model.Sell_Price,
                    Provider = provider,
                    IsDeleted = false,
                    UnitM = unitM
                };
                await productService.AddAsync(product);
                return RedirectToAction("ListPaginated");
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Por favor, selecciona un archivo Excel válido.";
                return RedirectToAction("ListPaginated");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    // Llamar al servicio para procesar el archivo Excel
                    string resultMessage = await excelService.ReadProductsFromExcel(stream);

                    if (string.IsNullOrWhiteSpace(resultMessage))
                    {
                        TempData["Error"] = "El archivo Excel no contiene productos válidos o ocurrió un error.";
                        return RedirectToAction("ListPaginated");
                    }

                    // Lógica para asignar NProduct
                    var lastAdded = await productService.LastAdded();  // Obtener el último producto agregado
                    int N = 1;  // Valor inicial para NProduct
                    if (lastAdded != null)
                    {
                        N = lastAdded.NProduct + 1;  // Incrementar el NProduct basándote en el último valor
                    }

                    // Aquí puedes acceder a los productos importados y asignar el NProduct
                    var products = await stockSaleDbContext.Products.ToListAsync(); // Obtén todos los productos importados o procesados
                    foreach (var product in products)
                    {
                        // Asignar el NProduct con el valor calculado
                        product.NProduct = N++;

                        // Aquí puedes agregar más lógica si necesitas hacer algo adicional con cada producto
                    }

                    // Guardar los cambios en la base de datos
                    await stockSaleDbContext.SaveChangesAsync();

                    // Mensaje de éxito
                    TempData["Success"] = resultMessage;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar el archivo: {ex.Message}";
            }

            return RedirectToAction("ListPaginated");
        }


        // Devuelve una vista que tiene el formulario para editar o eliminar un producto
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await productService.GetAsync(id);
            var providers = await providerService.GetAllAsync(); // Obtener proveedores
            var unitsM = await unitMRepository.GetAllAsync(); // Obtener unidades de medida
            
            if (product != null)
            {
                // Crear un ViewModel para pasar los datos a la vista
                var viewModel = new EditProductViewModel
                {
                    Id = product.Id,
                    IsDeleted = product.IsDeleted,
                    NProduct = product.NProduct,
                    Name = product.Name,
                    Packaging = product.Packaging,
                    Stock = product.Stock,
                    Stock_Limit = product.Stock_Limit,
                    List_Price = product.List_Price,
                    Sell_Price = product.Sell_Price,
                    Provider = product.Provider,
                    UnitM = product.UnitM,
                    Providers = providers,
                    UnitsM = unitsM
                };
                if (product.Provider != null)
                {
                    viewModel.ProviderId = product.Provider.Id;
                }
                if(product.UnitM != null)
                {
                    viewModel.UnitMId = product.UnitM.Id;
                }
                return View(viewModel);
            }
            return View(null);
        }

        // Envía el formulario con los nuevos datos y edita ese producto o devuelve un error
            [HttpPost]
            public async Task<IActionResult> Edit(EditProductViewModel editProductViewModel)
            {
                if (!ModelState.IsValid)
                {
                    // Recargar proveedores y unidades de medida si el formulario tiene errores
                    editProductViewModel.Providers = await providerService.GetAllAsync();
                    editProductViewModel.UnitsM = await unitMRepository.GetAllAsync();
                    return View(editProductViewModel);
                }

                // Crear el producto con los datos del ViewModel
                var productDomainModel = new Product
                {
                    Id = editProductViewModel.Id,
                    NProduct = editProductViewModel.NProduct,
                    Name = editProductViewModel.Name,
                    Packaging = editProductViewModel.Packaging,
                    Stock = editProductViewModel.Stock,
                    Stock_Limit = editProductViewModel.Stock_Limit,
                    List_Price = editProductViewModel.List_Price,
                    Sell_Price = editProductViewModel.Sell_Price,
                };

                if(editProductViewModel.ProviderId != null)
                {
                    var provider = await providerService.GetAsync(editProductViewModel.ProviderId.Value);
                    productDomainModel.Provider = provider;
                }
                if(editProductViewModel.UnitMId != null)
                {
                    var unitM = await unitMRepository.GetAsync(editProductViewModel.UnitMId.Value);
                    productDomainModel.UnitM = unitM;
                }

                var updatedProduct = await productService.UpdateAsync(productDomainModel);

                if (updatedProduct != null)
                {
                    // Redirigir a la lista de productos después de una actualización exitosa
                    return RedirectToAction("ListPaginated");
                }

                // Si algo sale mal, recargar proveedores y unidades de medida y devolver la vista con un mensaje de error
                editProductViewModel.Providers = await providerService.GetAllAsync();
                editProductViewModel.UnitsM = await unitMRepository.GetAllAsync();
                ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");

                return View(editProductViewModel);
            }


        // Envía el formulario para eliminar ese producto, recibe un id y borra el producto o devuelve un mensaje de error
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedProduct = await productService.DeleteAsync(id);
            if (deletedProduct != null)
            {
                // Mostrar un mensaje de éxito
                return RedirectToAction("ListPaginated");
            }

            // Mostrar un mensaje de error
            return RedirectToAction("Edit", new { id = id });
        }

[HttpPost]
public async Task<IActionResult> RecoverProduct(Guid id)
{
    // Intentar obtener el producto por su ID
    var product = await productService.GetAsync(id);
    if (product != null)
    {
        // Cambiar el estado de eliminado a falso
        product.IsDeleted = false;
        // Actualizar el producto en el repositorio
            var recoveredProduct = await productService.UpdateAsync(product);
            if(recoveredProduct != null)
        {
            return RedirectToAction("ListPaginated");
        }
        return RedirectToAction("Edit", new { id = id });
    }
    return RedirectToAction("ListPaginated");
}

        [HttpGet]
        public async Task<IActionResult> ListPaginated(
            string searchString,
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageSize = 10,
            int pageNumber = 1)
        {
            var allProducts = await productService.GetAllAsync();

            // Utilizar el servicio de paginado
            var paginatedResult = paginationService.GetPaginatedData(
                allProducts,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection,
                p => string.IsNullOrEmpty(searchString) ||
                     p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            );

            // Pasar datos a la vista
            ViewBag.VisiblePages = paginatedResult.VisiblePages;
            ViewBag.TotalPages = paginatedResult.TotalPages;
            ViewBag.PageNumber = paginatedResult.PageNumber;
            ViewBag.PageSize = paginatedResult.PageSize;

            return View(paginatedResult.Data);
        }
    }
}
