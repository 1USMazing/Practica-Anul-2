using Food_Order.Models;
using Food_Order.Services;
using Microsoft.AspNetCore.Mvc;

namespace Food_Order.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProduct createproduct)
        {
            if (createproduct.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFile", "Image file required");
            }

            if (!ModelState.IsValid)
            {
                return View(createproduct);
            }

            //Salvam imaginea
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(createproduct.ImageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/Images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                createproduct.ImageFileName.CopyTo(stream);
            }

            // salvam noul produs
            Product product = new Product()
            {
                Name = createproduct.Name,
                Description = createproduct.Description,
                Category = createproduct.Category,
                Price = createproduct.Price,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            // Adăugare produs în context și salvare
            context.Products.Add(product);
            context.SaveChanges();


            return RedirectToAction("Index", "Products");

        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // createproduct
            var createdproduct = new CreateProduct()
            {
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,

            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = DateTime.Now.ToString("MM/dd/yyyy");


            return View(createdproduct);
        }

        [HttpPost]
        public IActionResult Edit(int id, CreateProduct createproduct)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = DateTime.Now.ToString("MM/dd/yyyy");

                return View(createproduct);
            }

            // Verificăm dacă utilizatorul a încărcat o imagine nouă
            if (createproduct.ImageFileName != null)
            {
                // Ștergem imaginea veche, dacă există
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    string oldImagePath = Path.Combine(environment.WebRootPath, "Images", product.ImageFileName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Salvăm imaginea nouă
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(createproduct.ImageFileName.FileName);
                string imageFullPath = Path.Combine(environment.WebRootPath, "Images", newFileName);

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    createproduct.ImageFileName.CopyTo(stream);
                }

                // Actualizăm informațiile despre produs
                product.Name = createproduct.Name;
                product.Description = createproduct.Description;
                product.Category = createproduct.Category;
                product.Price = createproduct.Price;
                product.ImageFileName = newFileName;

                context.SaveChanges();

                return RedirectToAction("Index", "Products");
            }

            // Dacă nu s-a încărcat o imagine nouă, actualizăm doar celelalte informații despre produs
            product.Name = createproduct.Name;
            product.Description = createproduct.Description;
            product.Category = createproduct.Category;
            product.Price = createproduct.Price;

            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Delete(int id) 
        {
            var product = context.Products.Find(id);

            if (product == null) 
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/Images/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);

            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");

        }

    }
}
