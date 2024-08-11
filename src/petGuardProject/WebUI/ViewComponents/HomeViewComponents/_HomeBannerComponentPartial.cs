using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents.HomeViewComponents;
public class _HomeBannerComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var slides = new List<SlideModel>
        {
            new SlideModel {
                ImageUrl = "/assets/images/banner1.png",
                Title = "Sevimli Dostlarınız Güvende",
                Description = "Evcil hayvanlarınızın emin ellerde olduğunu bilerek gönül rahatlığıyla seyahate çıkın."
            },
            new SlideModel {
                ImageUrl = "/assets/images/banner2.jpeg",
                Title = "Profesyonel Bakım",
                Description = "Alanında uzman bakıcılarımız, evcil hayvanlarınıza en iyi bakımı sağlar."
            },
            new SlideModel {
                ImageUrl = "/assets/images/banner3.jpg",
                Title = "Her Türlü Hayvana Uygun Hizmet",
                Description = "Kedinizden köpeğinize, kuşunuzdan balığınıza kadar tüm evcil hayvanlar için hizmet sunuyoruz."
            }
        };

        return View(slides);
    }
}


public class _HomeFeaturesComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
