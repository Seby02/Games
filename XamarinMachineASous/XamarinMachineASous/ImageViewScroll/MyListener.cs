using Android.Animation;

namespace XamarinMachineASous.ImageViewScroll
{
    internal class MyListener : Java.Lang.Object, Animator.IAnimatorListener
    {
        private ImageViewScroll imageViewScroll;
        private int image;
        private int rotate_count;

        public MyListener(ImageViewScroll imageViewScroll, int image, int rotate_count)
        {
            this.imageViewScroll = imageViewScroll;
            this.image = image;
            this.rotate_count = rotate_count;
        }

        public void OnAnimationCancel(Animator animation)
        {
            
        }

        public void OnAnimationEnd(Animator animation)
        {
            imageViewScroll.SetImage(imageViewScroll.currentImage, imageViewScroll.old_value % 6); // 6 images
            imageViewScroll.currentImage.TranslationY = 0;
            if(imageViewScroll.old_value != rotate_count)
            {
                imageViewScroll.SetValueRandom(image, rotate_count);
                imageViewScroll.old_value++;

            } else
            {
                imageViewScroll.last_result = 0;
                imageViewScroll.old_value = 0;
                imageViewScroll.SetImage(imageViewScroll.nextImage, image);
                imageViewScroll.eventEnd.EventEnd(image % 6, rotate_count);
            }
        }

        public void OnAnimationRepeat(Animator animation)
        {
            
        }

        public void OnAnimationStart(Animator animation)
        {
            
        }
    }
}