using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace FFIV_Monster_Viewer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     
    public partial class MainWindow : Window {

        public struct Monsters {
            public string name;
            public decimal healthPoints;
            public decimal gold;
            public decimal exp;
            public int physPower;
            public int magiPower;
            public int physDefense;
            public int magiDefense;
            public string family;
            public string weakAgainst;
            public string strongAgainst;

        }//
        
        Monsters[] globalMonsterData;

        public MainWindow() {
            InitializeComponent();
        }//

        private void MonsterSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            //CHECK IF NO DATA HAS BEEN LOADED EXIT IF THIS IS THE CASE
            if (globalMonsterData == null) {
                MonsterSlide.Value = 0;
                return;
            }//end if

            //CREATE AN INT TO SNAP SLIDER VALUE TO
            int sliderInt = (int)MonsterSlide.Value;

            //UPDATE LABEL SHOWIN THE CURRENTLY SELECTED RECORD
            txtCounter.Text = sliderInt.ToString();

            //UPDATE FORM
            UpdateForm(sliderInt);

            //Update Image
            LoadImage(imgMain, $"C:\\Users\\MCA\\source\\repos\\FFIV Monster Viewer\\bin\\Debug\\net6.0-windows\\ffii_monster_images\\{txtMonstName.Text}.png");
        }//

        private void btnLoadFile_Click(object sender, RoutedEventArgs e) {
            string filePath = "C:\\Users\\MCA\\source\\repos\\FFIV Monster Viewer\\bin\\Debug\\net6.0-windows\\monster.csv";

            //GET RECORD COUNT
            int records = CountCsvRecords(filePath, true);

            //SET SLIDER TO MATCH
            MonsterSlide.Maximum = records - 1;

            //LOAD DATA FROM CSV & RETURN ARRAY OF PERSON
            globalMonsterData = ProcessCsvDataIntoMonsterStruct(filePath, records, true);

            //UPDATE FORM WITH 1ST PERSON'S DATA
            UpdateForm(0);

            //Display Image
            LoadImage(imgMain, $"C:\\Users\\MCA\\source\\repos\\FFIV Monster Viewer\\bin\\Debug\\net6.0-windows\\ffii_monster_images\\{txtMonstName.Text}.png");
        }

        private void backbtn_Click(object sender, RoutedEventArgs e) {
            MonsterSlide.Value--;
        }//

        private void forwardbtn_Click(object sender, RoutedEventArgs e) {
            MonsterSlide.Value++;
        }

        private void skipbackbtn_Click(object sender, RoutedEventArgs e) {
            MonsterSlide.Value = 0;
        }//

        private void skipforwardbtn_Click(object sender, RoutedEventArgs e) {
            MonsterSlide.Value = MonsterSlide.Maximum;
        }
        void LoadImage(Image imgTarget, string imagePath) {
            //Create a bitmap
            BitmapImage bmpImage = new BitmapImage();

            if (File.Exists(imagePath) == false) {
                imagePath = "C:\\Users\\MCA\\source\\repos\\FFIV Monster Viewer\\bin\\Debug\\net6.0-windows\\PhotoNotAvailable.jpg";
            }
            //Set bitmap for editing
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(imagePath); //Load the image
            bmpImage.EndInit();

           

            //Set the source of the image control to the bitmap 
            imgTarget.Source = bmpImage;
        }//end function

        int CountCsvRecords(string filePath, bool skipHeader) {
            //VARS
            int recordCount = 0;

            //OPEN THE FILE TO COUNT THE NUMBER OF RECORDS
            StreamReader infile = new StreamReader(filePath);

            //CONSUME HEADER WITH A READLINE
            if (skipHeader) {
                infile.ReadLine();
            }//end if

            //COUNT RECORDS
            while (infile.EndOfStream == false) {
                infile.ReadLine();
                recordCount += 1;
            }//end whle 

            //CLOSE FILE
            infile.Close();

            return recordCount;
        }//end function

        Monsters[] ProcessCsvDataIntoMonsterStruct(string filePath, int recordsToRead, bool skipHeader) {
            //VARS
            Monsters[] returnArray = new Monsters[recordsToRead];
            int currentRecordCount = 0;

            //OPEN THE FILE TO COUNT THE NUMBER OF RECORDS
            StreamReader infile = new StreamReader(filePath);

            //CONSUME HEADER WITH A READLINE
            if (skipHeader) {
                infile.ReadLine();
            }//end if

            //COUNT RECORDS
            while (infile.EndOfStream == false && currentRecordCount < recordsToRead) {
                string record = infile.ReadLine();
                string[] fields = record.Split(",");

                for (int index = 0; index < fields.GetLength(0); index++) {
                    if (fields[index] == "" || fields[index] == "?") {
                        fields[index] = "0";
                    }
                }

               returnArray[currentRecordCount].name = fields[0];
               returnArray[currentRecordCount].healthPoints = decimal.Parse(fields[1]);
               returnArray[currentRecordCount].gold = decimal.Parse(fields[2]);
               returnArray[currentRecordCount].exp = decimal.Parse(fields[3]);
               returnArray[currentRecordCount].physPower = int.Parse(fields[4]);
               returnArray[currentRecordCount].magiPower = int.Parse(fields[5]);
               returnArray[currentRecordCount].physDefense = int.Parse(fields[6]);
               returnArray[currentRecordCount].magiDefense = int.Parse(fields[7]);
               returnArray[currentRecordCount].family = fields[8];
               returnArray[currentRecordCount].weakAgainst = fields[9];
               returnArray[currentRecordCount].strongAgainst = fields[10];


                currentRecordCount += 1;
            }//end while 

            //CLOSE FILE
            infile.Close();

            return returnArray;
        }//end function

        void UpdateForm(int MonsterIndex) {
            //GRAB PERSON FROM THE GLOBAL ARRAY
            Monsters currentMonster = globalMonsterData[MonsterIndex];

            //UPDATE TEXBOXES ON THE FORM
            txtMonstName.Text          = currentMonster.name;
            txtMonstHealth.Text        = currentMonster.healthPoints.ToString();
            txtMonstGold.Text          = currentMonster.gold.ToString();
            txtMonstExperience.Text    = currentMonster.exp.ToString();
            txtMonstPhyicPower.Text    = currentMonster.physPower.ToString();
            txtMonstPhyisDefense.Text  = currentMonster.physDefense.ToString();
            txtMonstMagiPower.Text     = currentMonster.magiPower.ToString();
            txtMonstMagicDefense.Text  = currentMonster.magiDefense.ToString();
            txtMonstFamily.Text        = currentMonster.family;
            txtMonstWeakAgainst.Text   = currentMonster.weakAgainst;
            txtMonstStrongAgainst.Text = currentMonster.strongAgainst;

            if (txtMonstFamily.Text == "0") {
                txtMonstFamily.Text = "N/A";
            }//
            if (txtMonstWeakAgainst.Text == "0") {
                txtMonstWeakAgainst.Text = "N/A";
            }//
            if (txtMonstStrongAgainst.Text == "0") {
                txtMonstStrongAgainst.Text = "N/A";
            }
        }//end function
    } 
}
