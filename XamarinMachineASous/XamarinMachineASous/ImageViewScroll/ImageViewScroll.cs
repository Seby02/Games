using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XamarinMachineASous.ImageViewScroll
{
    public class ImageViewScroll:FrameLayout
    {
        private static int ANIMATION_DUR = 150;
        public ImageView currentImage, nextImage;
        public int last_result = 0, old_value = 0;
        public IEventEnd eventEnd;

        public void SetEventEnd(IEventEnd eventEnd)
        {
            this.eventEnd = eventEnd;
        }

        public ImageViewScroll(Context context) : base(context)
        {
            Init(context);
        }

        internal void SetImage(ImageView imageView, int value)
        {
            if (value == Utils.BAR)
                imageView.SetImageResource(Resource.Drawable.bar_done);
            else if (value == Utils.SEVEN)
                imageView.SetImageResource(Resource.Drawable.sevent_done);
            else if(value == Utils.LEMON)
                imageView.SetImageResource(Resource.Drawable.lemon_done);
            else if(value == Utils.ORANGE)
                imageView.SetImageResource(Resource.Drawable.orange_done);
            else if (value == Utils.TRIPLE)
                imageView.SetImageResource(Resource.Drawable.triple_done);
            else
                imageView.SetImageResource(Resource.Drawable.waternelon_done);

            imageView.Tag = value;
            last_result = value;
        }

        public int GetValue()
        {
            return int.Parse(nextImage.Tag.ToString());
        }


        public ImageViewScroll(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }


        private void Init(Context context)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.image_view_scroll, this);
            currentImage = FindViewById<ImageView>(Resource.Id.currentImage);
            nextImage = FindViewById<ImageView>(Resource.Id.nextImage);

            nextImage.TranslationY = (Height);
        }

        public void SetValueRandom(int image, int rotate_count)
        {
            currentImage.Animate().SetDuration(ANIMATION_DUR)
                .TranslationY(-Height)
                .Start();

            nextImage.TranslationY = nextImage.Height;

            nextImage.Animate().TranslationY(0).SetDuration(ANIMATION_DUR).SetListener(new MyListener(this, image, rotate_count));
        }
    }
}