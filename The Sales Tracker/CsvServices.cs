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

        private string _dataFilePath;

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
            string[] salespersonInfoArray;
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





            if (!Enum.TryParse<Product.ProductType>(salespersonInfoArray[3], out productType))
            {
                productType = Product.ProductType.None;
            }
            salesperson.CurrentStock.Type = productType;

            salesperson.CurrentStock.AddProducts(Convert.ToInt32(salespersonInfoArray[4]));
            salesperson.CurrentStock.OnBackorder = Convert.ToBoolean(salespersonInfoArray[5]); 

            salesperson.CitiesVisited = citiesTraveled.Split(',').ToList();

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
            sb.Append(salesperson.CurrentStock.Type.ToString() + delineator);
            sb.Append(salesperson.CurrentStock.NumberOfUnits.ToString() + delineator);
            sb.Append(salesperson.CurrentStock.OnBackorder.ToString());
            sb.Append(Environment.NewLine);

            //
            // add cities traveled to string
            //
            foreach (string city in salesperson.CitiesVisited)
            {
                sb.Append(city + delineator);
            }

            //
            // remove the last delineator
            //
            if (sb.Length !=0)
            {
                sb.Length--;
            }

            //
            // convert StringBuilder object to a string
            //
            salespersonData = sb.ToString();

            //
            // initialize a FileStream object for writing
            //
            FileStream wFileStream = File.OpenWrite(DataSettings.dataFilePathCsv);

            //
            // wrap the FileStream object in a using statement the ensure of the dispose
            //
       
                // wrap the FileStream object in a StreamWriter object to simplify writing strings\
                StreamWriter sWriter = new StreamWriter("Data\\Data.txt");

                using (sWriter)
                {
                    sWriter.Write(salespersonData);
                }            
        }

        #endregion
    }
}
