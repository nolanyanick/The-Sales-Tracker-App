using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Sales_Tracker
{
    public class CsvServices
    {
        #region Fields

        public string _dataFilePath;

        #endregion

        #region Properties

        public string DataFilePath
        {
            get { return _dataFilePath; }
            set { _dataFilePath = value; }
        }

        #endregion

        #region Constructors

        public CsvServices(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// reads data from Data.csv into a Salesperson object, then returns that object
        /// </summary>        
        public Salesperson ReadSalespersonFromDataFile()
        {
            Salesperson salesperson = new Salesperson();
            Product.ProductType productType = new Product.ProductType();

            string salespersonInfo;
            string[] salespersonInfoArray = new string[7];
            string citiesTraveled;

            //
            // initialize a FileStream object for writing
            //
            FileStream rFileStream = File.OpenRead(DataSettings.dataFilePathCsv);

            //
            // wrap the FileStream object in a using statement the ensure of the dispose
            //
            using (rFileStream)
            {
                // wrap the FileStream object in a StreamWriter object to simplify wrting strings\
                StreamReader sReader = new StreamReader(rFileStream);

                using (sReader)
                {
                    salespersonInfo = sReader.ReadLine();
                    citiesTraveled = sReader.ReadLine();
                }               
            }

            //
            // convert and write data to salesperson object
            //
            salespersonInfoArray = salespersonInfo.Split(',');
            salesperson.FirstName = salespersonInfoArray[0];
            salesperson.LastName = salespersonInfoArray[1];
            salesperson.AccountID = salespersonInfoArray[2];
            salesperson.StartingCity = salespersonInfoArray[3];

            if (!Enum.TryParse<Product.ProductType>(salespersonInfoArray[4], out productType))
            {
                productType = Product.ProductType.None;
            }
            salesperson.CurrentStock.Type = productType;

            salesperson.CurrentStock.AddProducts(Convert.ToInt32(salespersonInfoArray[5]));
            salesperson.CurrentStock.OnBackorder = Convert.ToBoolean(salespersonInfoArray[6]);            

            //
            // if citiesTraveled is null, then ignore
            //
            if (citiesTraveled == null)
            {
                return salesperson;
            }
            else
            {
                salesperson.CitiesVisited = citiesTraveled.Split(',').ToList();
            }
            
            return salesperson;
        }

        /// <summary>
        /// writes data from the Salesperson object into the Data.csv file
        /// </summary>        
        public void WriteSalespersonToDataFile(Salesperson salesperson)
        {
            string salespersonData;
            char delineator = ',';

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append(salesperson.FirstName + delineator);
            sb.Append(salesperson.LastName + delineator);
            sb.Append(salesperson.AccountID + delineator);
            sb.Append(salesperson.StartingCity + delineator);
            sb.Append(salesperson.CurrentStock.Type.ToString() + delineator);
            sb.Append(salesperson.CurrentStock.NumberOfUnits.ToString() + delineator);
            sb.Append(salesperson.CurrentStock.OnBackorder.ToString());
            sb.Append(Environment.NewLine);

            //
            // add cities traveled to sb
            //
            foreach (string city in salesperson.CitiesVisited)
            {
                sb.Append(city + delineator);
            }

            //
            // remove the last delineator
            //
            if (sb.Length != 0)
            {
                sb.Length--;
            }

            //
            // convert StringBuilder object to a string
            //
            salespersonData = sb.ToString();

            try
            {                
                using (StreamWriter sWriter = new StreamWriter(DataSettings.dataFilePathCsv, false))
                {
                    sWriter.Write(salespersonData);
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                //
                // create a directory if one is not already present
                //
                Directory.CreateDirectory("Data");

                using (StreamWriter sWriter = new StreamWriter(DataSettings.dataFilePathCsv, false))
                {
                    sWriter.Write(salespersonData);
                }
            }
            finally
            {

            }
        }

        #endregion
    }
}
