using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace V电影.Composition
{
    public class Animation
    {
        private static Resource.APPTheme apptheme = new Resource.APPTheme();

        public static void Drop_Shadow(UIElement host, UIElement target)
        {
            Visual hostvisual = ElementCompositionPreview.GetElementVisual(target);
            Compositor compositor = hostvisual.Compositor;

            SpriteVisual spritevisual = compositor.CreateSpriteVisual();
            ElementCompositionPreview.SetElementChildVisual(host, spritevisual);

            DropShadow dropshadow = compositor.CreateDropShadow();
            dropshadow.Color = apptheme.Gary_Color_Brush.Color;
            dropshadow.Opacity = 0.5f;
            dropshadow.Offset = new System.Numerics.Vector3(0f, 5f, 0f);
            if (target is Shape)
            {
                dropshadow.Mask = (target as Shape).GetAlphaMask();
            }
            spritevisual.Shadow = dropshadow;

            ExpressionAnimation animation = compositor.CreateExpressionAnimation("hostvisual.Size");
            animation.SetReferenceParameter("hostvisual", hostvisual);
            spritevisual.StartAnimation("Size", animation);
        }
    }
}
