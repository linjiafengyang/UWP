using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Cashbook.Models
{
    class PersonalInfo
    {
        public string id;
        public string nickname { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public BitmapImage picture { get; set; }
        public Uri imageUri { get; set; }

        public PersonalInfo(string id, string nickname, string mail, string password, string imageUriString)
        {
            this.id = Guid.NewGuid().ToString();
            this.nickname = nickname;
            this.mail = mail;
            this.password = password;
            this.picture = picture;
            this.imageUri = new Uri(imageUriString);
            if (imageUriString == "ms-appx:///Assets/background.jpg")
            {
                this.picture = new BitmapImage(this.imageUri);
            }
            else
            {
                BitmapImage bi = new BitmapImage(new Uri(imageUri.LocalPath));
                this.picture = bi;
            }
        }
        public void addPersonalInfo(string id, string nickname, string mail, string password, string imageUriString)
        {
            this.id = Guid.NewGuid().ToString();
            this.nickname = nickname;
            this.mail = mail;
            this.password = password;
            this.imageUri = new Uri(imageUriString);
        }
    }
}
