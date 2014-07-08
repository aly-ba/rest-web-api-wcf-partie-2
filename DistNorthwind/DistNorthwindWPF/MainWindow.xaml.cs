using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DistNorthwindWPF.ProductServiceProxy;
using System.Transactions;  

namespace DistNorthwindWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Product product1, product2;
        RemoteProductServiceProxy.Product remoteProduct;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGetProduct_Click(object sender, RoutedEventArgs e)
        {
            txtProduct1Details.Text = GetProduct(
                   txtProductID1, ref product1);
            if (chkRemote.IsChecked == true)
                txtProduct2Details.Text = GetRemoteProduct(
                    txtProductID2, ref remoteProduct);
            else
                txtProduct2Details.Text = GetProduct(
                    txtProductID2, ref product2);
        }

        private string GetProduct(TextBox txtProductID,
                          ref Product product)
        {
            string result = "";

            try
            {
                int productID = Int32.Parse(txtProductID.Text);
                var client = new ProductServiceClient();
                product = client.GetProduct(productID);

                var sb = new StringBuilder();
                sb.Append("ProductID:" +
                    product.ProductID.ToString() + "\n");
                sb.Append("ProductName:" +
                    product.ProductName + "\n");
                sb.Append("UnitPrice:" +
                    product.UnitPrice.ToString() + "\n");
                sb.Append("RowVersion:");
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }
                result = sb.ToString();

            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message.ToString();
            }

            return result;
        }

        private string GetRemoteProduct(TextBox txtProductID,
            ref RemoteProductServiceProxy.Product product)
        {
            string result = "";

            try
            {
                int productID = Int32.Parse(txtProductID.Text);
                var client =
                    new RemoteProductServiceProxy.ProductServiceClient();
                product = client.GetProduct(productID);

                var sb = new StringBuilder();
                sb.Append("ProductID:" +
                    product.ProductID.ToString() + "\n");
                sb.Append("ProductName:" +
                    product.ProductName + "\n");
                sb.Append("UnitPrice:" +
                    product.UnitPrice.ToString() + "\n");
                sb.Append("RowVersion:");
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }
                result = sb.ToString();

            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message.ToString();
            }

            return result;
        }

        private void btnUpdatePrice_Click(object sender,
                                          RoutedEventArgs e)
        {
            if (product1 == null)
            {
                txtUpdate1Results.Text = "Get product details first";
            }
            else if (chkRemote.IsChecked == false && product2 == null ||
                chkRemote.IsChecked == true && remoteProduct == null)
            {
                txtUpdate2Results.Text = "Get product details first";
            }
            else
            {
                bool update1Result = false, update2Result = false;

                using (var ts = new TransactionScope())
                {
                    txtUpdate1Results.Text = UpdatePrice(
                            txtNewPrice1,
                            ref product1,
                            ref update1Result);
                    if (chkRemote.IsChecked == true)
                        txtUpdate2Results.Text = UpdateRemotePrice(
                            txtNewPrice2,
                            ref remoteProduct,
                            ref update2Result);
                    else
                        txtUpdate2Results.Text = UpdatePrice(
                            txtNewPrice2,
                            ref product2,
                            ref update2Result);
                    if (update1Result == true && update2Result == true)
                        ts.Complete();
                }
            }
        }

        private string UpdatePrice(
            TextBox txtNewPrice,
            ref Product product,
            ref bool updateResult)
        {
            string result = "";
            string message = "";

            try
            {
                product.UnitPrice =
                    Decimal.Parse(txtNewPrice.Text);

                var client =
                    new ProductServiceClient();
                updateResult =
                    client.UpdateProduct(ref product, ref message);
                var sb = new StringBuilder();

                if (updateResult == true)
                {
                    sb.Append("Price updated to ");
                    sb.Append(txtNewPrice.Text.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("Update message:");
                    sb.Append(message);
                    sb.Append("\n");
                    sb.Append("New RowVersion:");
                }
                else
                {
                    sb.Append("Price not updated to ");
                    sb.Append(txtNewPrice.Text.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("Update message:");
                    sb.Append(message);
                    sb.Append("\n");
                    sb.Append("Old RowVersion:");
                }
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }

                result = sb.ToString();
            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message;
            }

            return result;
        }

        private string UpdateRemotePrice(
            TextBox txtNewPrice,
            ref RemoteProductServiceProxy.Product product,
            ref bool updateResult)
        {
            string result = "";
            string message = "";

            try
            {
                product.UnitPrice =
                    Decimal.Parse(txtNewPrice.Text);

                var client =
                    new RemoteProductServiceProxy.ProductServiceClient();
                updateResult =
                    client.UpdateProduct(ref product, ref message);
                var sb = new StringBuilder();

                if (updateResult == true)
                {
                    sb.Append("Price updated to ");
                    sb.Append(txtNewPrice.Text.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("Update message:");
                    sb.Append(message);
                    sb.Append("\n");
                    sb.Append("New RowVersion:");
                }
                else
                {
                    sb.Append("Price not updated to ");
                    sb.Append(txtNewPrice.Text.ToString());
                    sb.Append("\n");
                    sb.Append("Update result:");
                    sb.Append(updateResult.ToString());
                    sb.Append("\n");
                    sb.Append("Update message:");
                    sb.Append(message);
                    sb.Append("\n");
                    sb.Append("Old RowVersion:");
                }
                foreach (var x in product.RowVersion.AsEnumerable())
                {
                    sb.Append(x.ToString());
                    sb.Append(" ");
                }

                result = sb.ToString();
            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message;
            }

            return result;
        }

    }
}
