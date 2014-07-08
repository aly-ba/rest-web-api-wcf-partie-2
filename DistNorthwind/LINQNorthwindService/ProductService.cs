using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using LINQNorthwindLogic;
using LINQNorthwindBDO;

namespace LINQNorthwindService
{
    public class ProductService : IProductService
    {
        ProductLogic productLogic = new ProductLogic();

        public Product GetProduct(int id)
        {
            ProductBDO productBDO = null;
            try
            {
                productBDO = productLogic.GetProduct(id);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                string reason = "GetProduct Exception";
                throw new FaultException<ProductFault>
                    (new ProductFault(msg), reason);
            }

            if (productBDO == null)
            {
                string msg =
                    string.Format("No product found for id {0}",
                    id);
                string reason = "GetProduct Empty Product";
                throw new FaultException<ProductFault>
                    (new ProductFault(msg), reason);
            }

            Product product = new Product();
            TranslateProductBDOToProductDTO(productBDO, product);
            return product;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public bool UpdateProduct(ref Product product,
            ref string message)
        {
            bool result = true;

            // ProductName can't be empty
            if (string.IsNullOrEmpty(product.ProductName))
            {
                message = "Product name cannot be empty";
                result = false;
            }
            // QuantityPerUnit can't be empty
            else if (string.IsNullOrEmpty(product.QuantityPerUnit))
            {
                message = "Quantity cannot be empty";
                result = false;
            }
            else
            {
                try
                {
                    var productBDO = new ProductBDO();
                    TranslateProductDTOToProductBDO(product, productBDO);
                    result = productLogic.UpdateProduct(
                        ref productBDO, ref message);
                    product.RowVersion = productBDO.RowVersion;
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                    throw new FaultException<ProductFault>
                        (new ProductFault(msg), msg);
                }
            }
            return result;
        }

        private void TranslateProductBDOToProductDTO(
            ProductBDO productBDO,
            Product product)
        {
            product.ProductID = productBDO.ProductID;
            product.ProductName = productBDO.ProductName;
            product.QuantityPerUnit = productBDO.QuantityPerUnit;
            product.UnitPrice = productBDO.UnitPrice;
            product.Discontinued = productBDO.Discontinued;
            product.RowVersion = productBDO.RowVersion;
        }

        private void TranslateProductDTOToProductBDO(
            Product product,
            ProductBDO productBDO)
        {
            productBDO.ProductID = product.ProductID;
            productBDO.ProductName = product.ProductName;
            productBDO.QuantityPerUnit = product.QuantityPerUnit;
            productBDO.UnitPrice = product.UnitPrice;
            productBDO.Discontinued = product.Discontinued;
            productBDO.RowVersion = product.RowVersion;
        }

    }
}
