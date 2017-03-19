using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VK_api.Properties;
using VkNet;

namespace VK_api
{
    public partial class Form1 : Form
    {
        VkApi vkApi;

        ulong appID = 5934052;                      // ID приложения
        Settings scope = Settings.Default;      // Приложение имеет доступ к друзьям

        public Form1()
        {
            InitializeComponent();

        }

        string getCode()
        {
            Form2 code = new Form2();
            code.ShowDialog();

            return code.textBox1.Text;
        }

        /*Func<string> code = () =>
        {
            //Console.Write("Please enter code: ");
            string value = textBox1.Text;

            return value;
        };*/

    private void button1_Click(object sender, EventArgs e)
        {
            vkApi = new VkApi();
            vkApi.Authorize(new ApiAuthParams
            {
                ApplicationId = appID,
                Login = emailTextBox.Text,
                Password = passwordTextBox.Text,
                TwoFactorAuthorization = getCode
            });
            MessageBox.Show("Authorized!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            var user1 = vkApi.Utils.ResolveScreenName(textBox1.Text);
            var user2 = vkApi.Utils.ResolveScreenName(textBox2.Text);

            var friends1 = vkApi.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
            {
                UserId = user1.Id
            });

            var friends2 = vkApi.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
            {
                UserId = user2.Id
            });

            var friends1Ids = new List<long>();
            foreach (var friend in friends1)
            {
                friends1Ids.Add(friend.Id);
            }

            var friends2Ids = new List<long>();
            foreach (var friend in friends2)
            {
                friends2Ids.Add(friend.Id);
            }

            var commonFriends = friends1Ids.Intersect(friends2Ids);
            foreach( var friend in commonFriends)
            {
                richTextBox1.Text += friend + "\n";
            }
        }
    }
}
