using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using XamarinMachineASous.ImageViewScroll;
using System;

namespace XamarinMachineASous
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IEventEnd
    {

        ImageView btn_up, btn_down;
        ImageViewScroll.ImageViewScroll image, image2, image3;
        TextView txt_score;

        int count_down = 0;

        public void EventEnd(int value, int count)
        {
            if (count_down < 2)
                count_down++;
            else
            {
                btn_down.Visibility = Android.Views.ViewStates.Gone;
                btn_up.Visibility = Android.Views.ViewStates.Visible;

                count_down = 0;

                if (image.GetValue() == image2.GetValue() && image2.GetValue() == image3.GetValue())
                {
                    Toast.MakeText(this, "Vous avez gagné un gros prix", ToastLength.Short).Show();
                    Common.SCORE += 300;
                    txt_score.Text = Common.SCORE.ToString();
                }
                else if (image.GetValue() == image2.GetValue() || image2.GetValue() == image3.GetValue()
                    || image.GetValue() == image3.GetValue())
                {
                    Toast.MakeText(this, "Vous avez gagné un petit prix", ToastLength.Short).Show();
                    Common.SCORE += 100;
                    txt_score.Text = Common.SCORE.ToString();
                }
                else
                {
                    Toast.MakeText(this, "Vous avez perdu", ToastLength.Short).Show();
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;

            btn_down = FindViewById<ImageView>(Resource.Id.down);
            btn_up = FindViewById<ImageView>(Resource.Id.up);

            image = FindViewById<ImageViewScroll.ImageViewScroll>(Resource.Id.image);
            image2 = FindViewById<ImageViewScroll.ImageViewScroll>(Resource.Id.image2);
            image3 = FindViewById<ImageViewScroll.ImageViewScroll>(Resource.Id.image3);

            txt_score = FindViewById<TextView>(Resource.Id.txt_score);

            image.SetEventEnd(this);
            image2.SetEventEnd(this);
            image3.SetEventEnd(this);

            btn_up.Click += delegate
            {
                if (Common.SCORE >= 50)
                {
                    btn_up.Visibility = Android.Views.ViewStates.Gone;
                    btn_down.Visibility = Android.Views.ViewStates.Visible;

                    image.SetValueRandom(new Random(DateTime.Now.Millisecond).Next(6),
                        new Random(DateTime.Now.Millisecond).Next(5, 16));
                    image2.SetValueRandom(new Random(DateTime.Now.Millisecond).Next(6),
                        new Random(DateTime.Now.Millisecond).Next(5, 16));
                    image3.SetValueRandom(new Random(DateTime.Now.Millisecond).Next(6),
                        new Random(DateTime.Now.Millisecond).Next(5, 16));

                    Common.SCORE -= 50;
                    txt_score.Text = Common.SCORE.ToString();
                }

                else
                {
                    Toast.MakeText(this, "Vous n'avez pas assez d'argent", ToastLength.Short).Show();
                }
            };
        }
    }
}