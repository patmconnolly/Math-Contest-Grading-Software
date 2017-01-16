using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Math_Contest_Grading_Software
{
    public partial class MCG : Form
    {
        string theMonth;
        string theDay;
        string theYear;

        #region Variable_Defs

        string theDate;

        string JuniorKey;     //Junior Key String
        string SeniorKey;     //Senior Key String
        string JuniorTie;     //Junior Tie String
        string SeniorTie;     //Senior Tie String
        bool JKFault = false;           //Junior Key Fault
        bool SKFault = false;           //Senior Key Fault
        bool JTFault = false;           //Junior Tie Fault
        bool STFault = false;           //Senior Tie Fault
        List<Student> Junior = new List<Student>();   //List for junior data
        List<Student> Senior = new List<Student>();   //List for senior data
        List<School> SchoolList = new List<School>();         //List for school data

        List<int> SchNums = new List<int>();
        List<string> SchNams = new List<string>();

        List<string> JunErrorList = new List<string>();      //List of all Juniors and saying ok or error.
        List<string> SenErrorList = new List<string>();      //List of all Seniors and saying ok or error.

        string SeniorFile;
        string JuniorFile;
        string SchoolListFile;

        bool valid = false;

        List<string> instructorOutput;
        #endregion Variable_Defs

        public MCG()
        {
            InitializeComponent();
            month.Text = DateTime.Now.ToString("MMMM");
            day.Text = DateTime.Now.ToString("dd");
            year.Text = DateTime.Now.ToString("yyyy");
        }

        private void validateGrade_Click(object sender, EventArgs e)
        {

            string AppPath;
            theMonth = month.Text;
            theDay = day.Text;
            theYear = year.Text;
            theDate = theMonth + " " + theDay + ", " + theYear;
            
            getSchNumList();

            populate();

            valid = validation();
            if (valid)
            {
                gradeIt();

                assignSchool();

                cleanSchool();

                SchoolOutputStrings();

                masterOutput();

                //ShowPopUpMsg(teamJString());
                //writeToFile(SchoolList[0].getOutput(), SchoolList[0].returnName());
                //ShowPopUpMsg(teamSString());
                
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if(fbd.ShowDialog()==DialogResult.OK)
                {
                    AppPath = fbd.SelectedPath;
                    AppPath += "\\";

                    //writeToFile(List<string> lines, string fileName, string path)
                    //File.WriteAllLines(AppPath + "INSTRUCTOR_INFORMATION" + ".txt", instructorOutput);
                    writeToFile(instructorOutput, "INSTRUCTOR_INFORMATION", AppPath);
                    //Write everything to files
                    for (int i = 0; i < SchoolList.Count; i++)
                    {
                        if (SchoolList[i].returnJScore() > 0 || SchoolList[i].returnSScore() > 0)
                        {
                            //File.WriteAllLines(AppPath + SchoolList[i].returnName() + ".txt", SchoolList[i].getOutput());
                            writeToFile(SchoolList[i].getOutput(), SchoolList[i].returnName(), AppPath);
                        }
                    }
                    MessageBox.Show("SUCCESS!\n\nCheck Folder for Output Files");
                }
                else
                {
                    MessageBox.Show("INVALID PATH!\nPLEASE TRY AGAIN...");
                }
            }
            else
            {
                assignSchool();

                cleanSchool();

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    AppPath = fbd.SelectedPath;
                    AppPath += "\\";

                    //writeToFile(List<string> lines, string fileName, string path)
                    //File.WriteAllLines(AppPath + "INSTRUCTOR_INFORMATION" + ".txt", instructorOutput);
                    writeToFile(ComboValidationString(), "ValidationFile", AppPath);
                    
                    MessageBox.Show("Validation File Produced\nPlease Correct Errors And Try Again.");
                }
                else
                {
                    MessageBox.Show("INVALID PATH!\nPLEASE TRY AGAIN...");
                }
            }
        }

        private void lowerFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".txt Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if(ofd.ShowDialog()==DialogResult.OK)
            {
                JuniorFile = ofd.FileName;
                LFileBox.Text = ofd.SafeFileName;
                LFileBox.Update();
            }
            else
            {
                MessageBox.Show("Unreadable Lower File!\nPlease Select a Different File.");
            }
        }

        private void upperFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".txt Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SeniorFile = ofd.FileName;
                UFileBox.Text = ofd.SafeFileName;
                UFileBox.Update();
            }
            else
            {
                MessageBox.Show("Unreadable Upper File!\nPlease Select a Different File.");
            }
        }

        private void schoolFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".txt Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SchoolListFile = ofd.FileName;
                SFileBox.Text = ofd.SafeFileName;
                SFileBox.Update();
            }
            else
            {
                MessageBox.Show("Unreadable School File!\nPlease Select a Different File.");
            }
        }
        
        #region Non Class Validation

        public void validateKey(char level, List<string> theLine)
        {
            int item = theLine.Count() - 1;
            bool issue = false;
            if (theLine[item].Length != 40)
            {
                issue = true;
                //ShowPopUpMsg("Length Error In Key" + level.ToString());
            }
            else
            {
                for (int i = 0; i < 40; i++)
                {
                    if (theLine[item][i] != '1' && theLine[item][i] != '2' && theLine[item][i] != '3' && theLine[item][i] != '4' && theLine[item][i] != '5')
                    {
                        issue = true;
                        //ShowPopUpMsg("Value Error In Key" + level.ToString());
                    }
                }
            }


            if (level == 'J')
            {
                JuniorKey = theLine[item];
                JKFault = issue;
            }
            else
            {
                SeniorKey = theLine[item];
                SKFault = issue;
            }
        }

        public void validateTie(char level, List<string> theLine)
        {
            int item = theLine.Count() - 1;
            bool issue = false;
            if (theLine[item].Length != 40)
            {
                issue = true;
            }
            else
            {
                for (int i = 0; i < 40; i++)
                {
                    if (theLine[item][i] != '1' && theLine[item][i] != '2' && theLine[item][i] != '*')
                    {
                        issue = true;
                    }
                }
            }
            if (level == 'J')
            {
                JuniorTie = theLine[item];
                JTFault = issue;
            }
            else
            {
                SeniorTie = theLine[item];
                STFault = issue;
            }
        }

        #endregion Non Class Validation


        #region Helper Functions

        #region Key Functions

        public void populate()
        {
            //Input the file by line and save into a list one for junior and senior
            List<string> SenFile = File.ReadAllLines(SeniorFile).ToList();
            List<string> JunFile = File.ReadAllLines(JuniorFile).ToList();
            List<string> SchoolFile = File.ReadAllLines(SchoolListFile).ToList();

            SchoolList.Add(new School(killWhiteSpace("-1 UNKNOWN SCHOOL")));
            for (int i = 0; i < SchoolFile.Count; i++)
            {
                SchoolList.Add(new School(killWhiteSpace(SchoolFile[i])));        //Parses school list and creates the school elements
            }

            //Does the key and tiebreakers then populates the student files
            validateKey('J', killWhiteSpace(JunFile[0]));
            validateTie('J', killWhiteSpace(JunFile[1]));
            for (int i = 2; i < JunFile.Count(); i++)
            {
                Junior.Add(new Student(killWhiteSpace(JunFile[i]), SchNums));
            }
            validateKey('S', killWhiteSpace(SenFile[0]));
            validateTie('S', killWhiteSpace(SenFile[1]));
            for (int i = 2; i < SenFile.Count(); i++)
            {
                Senior.Add(new Student(killWhiteSpace(SenFile[i]), SchNums));
            }
        }

        public bool validation()
        {
            for (int i = 0; i < Junior.Count; i++)
            {
                if (Junior[i].returnErr()) { return false; }
            }
            for (int i = 0; i < Senior.Count; i++)
            {
                if (Senior[i].returnErr()) { return false; }
            }
            return true;
        }

        public void assignSchool()
        {
            bool flipped;
            assignSchoolToJKid();
            assignSchoolToSKid();
            for (int i = 0; i < Junior.Count; i++)
            {
                flipped = false;
                for (int j = 0; j < SchoolList.Count; j++)
                {
                    if (Junior[i].returnschoolCode() == SchoolList[j].returnCode())
                    {
                        SchoolList[j].addJunior(Junior[i]);
                        Junior[i].setSchoolName(SchoolList[j].returnName());
                        flipped = true;
                        break;
                    }
                }
                if (!flipped) { Junior[i].setSchoolNameError(true); }
            }

            for (int i = 0; i < Senior.Count; i++)
            {
                flipped = false;
                for (int j = 0; j < SchoolList.Count; j++)
                {
                    if (Senior[i].returnschoolCode() == SchoolList[j].returnCode())
                    {
                        SchoolList[j].addSenior(Senior[i]);
                        Senior[i].setSchoolName(SchoolList[j].returnName());
                        flipped = true;
                        break;
                    }
                }
                if (!flipped) { Senior[i].setSchoolNameError(true); }
            }
        }

        public void cleanSchool()
        {
            for (int i = SchoolList.Count; i > 0; i--)
            {
                if (!SchoolList[i - 1].isUsed())
                {
                    SchoolList.RemoveAt(i - 1);
                }
                else
                {
                    SchoolList[i - 1].calcTeamScore();
                }
            }
        }

        protected double calcGrade(string answers, string key, string tie)
        {

            double it = 0;

            for (int i = 0; i < 40; i++)
            {
                if (answers[i] == key[i])
                {
                    it = it + 1;
                    if (tie[i] == '1') { it = it + 0.01; }
                    else if (tie[i] == '2') { it = it + 0.0001; }
                }
            }
            return it;
        }

        public void gradeIt()
        {
            for (int i = 0; i < Junior.Count; i++)
            {
                if (!(Junior[i].returnErr()) && !JKFault) { Junior[i].setscore(calcGrade(Junior[i].returnanswers(), JuniorKey, JuniorTie)); }
            }

            for (int i = 0; i < Senior.Count; i++)
            {
                if (!(Senior[i].returnErr()) && !SKFault) { Senior[i].setscore(calcGrade(Senior[i].returnanswers(), SeniorKey, SeniorTie)); }
            }
        }

        public List<string> killWhiteSpace(string line)     //Takes in a string, removes white space and returns a list of strings, use like .split()
        {
            List<string> theLine = new List<string>();
            string theWord = "";
            int counter = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != ' ' && line[i] != '\t')
                {
                    theWord += line[i].ToString();
                    counter++;
                }
                else if ((line[i] == ' ' || line[i] == '\t') && counter > 0)
                {
                    theLine.Add(theWord);
                    theWord = "";
                    counter = 0;
                }
            }
            theLine.Add(theWord);
            return theLine;
        }

        private void bubbleSortStudent()
        {
            Student temp;

            for (int i = Junior.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (Junior[j].returnscore() < Junior[j + 1].returnscore())
                    {
                        temp = Junior[j];
                        Junior[j] = Junior[j + 1];
                        Junior[j + 1] = temp;
                    }
                }
            }

            for (int i = Senior.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (Senior[j].returnscore() < Senior[j + 1].returnscore())
                    {
                        temp = Senior[j];
                        Senior[j] = Senior[j + 1];
                        Senior[j + 1] = temp;
                    }
                }
            }
        }

        private void bubbleSortJSchool()
        {
            School temp;

            for (int i = SchoolList.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (SchoolList[j].returnJScore() < SchoolList[j + 1].returnJScore())
                    {
                        temp = SchoolList[j];
                        SchoolList[j] = SchoolList[j + 1];
                        SchoolList[j + 1] = temp;
                    }
                }
            }
        }

        private void bubbleSortSSchool()
        {
            School temp;

            for (int i = SchoolList.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (SchoolList[j].returnSScore() < SchoolList[j + 1].returnSScore())
                    {
                        temp = SchoolList[j];
                        SchoolList[j] = SchoolList[j + 1];
                        SchoolList[j + 1] = temp;
                    }
                }
            }
        }

        private void getSchNumList()
        {
            for (int i = 0; i < SchoolList.Count; i++)
            {
                SchNums.Add(SchoolList[i].returnCode());
                SchNams.Add(SchoolList[i].returnName());
            }
        }

        private void assignSchoolToJKid()
        {
            bool set;
            for (int i = 0; i < Junior.Count; i++)
            {
                set = false;
                for (int j = 0; j < SchNums.Count; j++)
                {
                    if (Junior[i].returnschoolCode() == SchNums[j])
                    {
                        Junior[i].setSchoolName(SchNams[j]);
                        set = true;
                        break;
                    }
                }
                if (!set) { Junior[i].setSchoolName("UNKNOWN SCHOOL"); }
            }
        }

        private void assignSchoolToSKid()
        {
            bool set;
            for (int i = 0; i < Senior.Count; i++)
            {
                set = false;
                for (int j = 0; j < SchNums.Count; j++)
                {
                    if (Senior[i].returnschoolCode() == SchNums[j])
                    {
                        Senior[i].setSchoolName(SchNams[j]);
                        set = true;
                        break;
                    }
                }
                if (!set) { Senior[i].setSchoolName("UNKNOWN SCHOOL"); }
            }
        }

        private int returnAnnual()
        {
            int it = parseNumber(DateTime.Now.ToString("yyyy"));
            return it - 1972;
        }

        private int parseNumber(string it)
        {
            int theNumber = 0;
            for (int i = 0; i < it.Length; i++)
            {
                int temp = parseNumHelper(it[i]);
                if (temp >= 0)
                {
                    theNumber = (theNumber * 10) + temp;
                }
                else { return -1; }
            }
            return theNumber;
        }

        private int parseNumHelper(char it)         //Expects positive integars in char form, if not valid returns -1
        {
            if (it == '0') { return 0; }
            else if (it == '1') { return 1; }
            else if (it == '2') { return 2; }
            else if (it == '3') { return 3; }
            else if (it == '4') { return 4; }
            else if (it == '5') { return 5; }
            else if (it == '6') { return 6; }
            else if (it == '7') { return 7; }
            else if (it == '8') { return 8; }
            else if (it == '9') { return 9; }
            else { return -1; }
        }

        private string getAnnualPostFix()
        {
            int it = returnAnnual();
            if (it % 10 == 1) { return it.ToString() + "st"; }
            else if (it % 10 == 2) { return it.ToString() + "st"; }
            else if (it % 10 == 3) { return it.ToString() + "rd"; }
            else { return it.ToString() + "th"; }
        }
        #endregion Key Functions
        #region Debugging Functions

        private void ShowPopUpMsg(string msg)
        {
            MessageBox.Show(msg);
            /*StringBuilder sb = new StringBuilder();
            sb.Append("alert('");
            sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
            sb.Append("');");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);*/
        }

        public void printErrBoxes()     //Pops up a message box with the errors, this is a bit buggy
        {
            string JString = "";
            string SString = "";
            for (int i = 0; i < JunErrorList.Count; i++)
            {
                JString = JString + JunErrorList[i] + "\n";
            }
            for (int i = 0; i < SenErrorList.Count; i++)
            {
                SString = SString + SenErrorList[i] + "\n";
            }
            ShowPopUpMsg(JString);
            ShowPopUpMsg(SString);
        }

        #endregion Debugging Functions
        #region String Builder Functions

        public List<string> JValidationString()
        {
            List<string> it = new List<string>();
            int numErr = 0;
            int numStud = 0;

            //Counts the errors
            for (int i = 0; i < Junior.Count; i++)
            {
                numStud++;
                if (Junior[i].returnErr()) { numErr++; }
            }

            //Key and tie error
            if (JKFault) { it.Add("LOWER KEY ERROR     " + JuniorKey); }
            else { it.Add("LOWER KEY ~OK~      " + JuniorKey); }

            if (JTFault) { it.Add("LOWER TIE ERROR     " + JuniorTie); }
            else { it.Add("LOWER TIE ~OK~      " + JuniorTie); }

            //Number of students/error students
            it.Add("TOTAL: " + numStud.ToString() + " STUDENTS | ERRORS: " + numErr.ToString() + " STUDENTS");

            //Prints out the error string to the string
            for (int i = 0; i < Junior.Count; i++)
            {
                it.Add(Junior[i].debugString());
            }

            return it;
        }

        public List<string> SValidationString()
        {
            List<string> it = new List<string>();
            int numErr = 0;
            int numStud = 0;

            //Counts the errors
            for (int i = 0; i < Senior.Count; i++)
            {
                numStud++;
                if (Senior[i].returnErr()) { numErr++; }
            }

            //Key and tie error
            if (SKFault) { it.Add("UPPER KEY ERROR     " + SeniorKey); }
            else { it.Add("UPPER KEY ~OK~      " + SeniorKey); }

            if (STFault) { it.Add("UPPER TIE ERROR     " + SeniorTie); }
            else { it.Add("UPPER TIE ~OK~      " + SeniorTie); }

            //Number of students/error students
            it.Add("TOTAL: " + numStud.ToString() + " STUDENTS | ERRORS: " + numErr.ToString() + " STUDENTS");

            //Prints out the error string to the string
            for (int i = 0; i < Senior.Count; i++)
            {
                it.Add(Senior[i].debugString());
            }

            return it;
        }

        public List<string> ComboValidationString()
        {
            List<string> it = new List<string>();
            int numErr = 0;
            int numStud = 0;

            //Counts the errors
            for (int i = 0; i < Junior.Count; i++)
            {
                numStud++;
                if (Junior[i].returnErr()) { numErr++; }
            }

            //Key and tie error
            if (JKFault) { it.Add("LOWER KEY ERROR     " + JuniorKey); }
            else { it.Add("LOWER KEY ~OK~      " + JuniorKey); }

            if (JTFault) { it.Add("LOWER TIE ERROR     " + JuniorTie); }
            else { it.Add("LOWER TIE ~OK~      " + JuniorTie); }

            //Number of students/error students
            it.Add("TOTAL: " + numStud.ToString() + " STUDENTS | ERRORS: " + numErr.ToString() + " STUDENTS");

            //Prints out the error string to the string
            for (int i = 0; i < Junior.Count; i++)
            {
                it.Add(Junior[i].debugString());
            }

            //Add Page Break
            it.Add("\f");
            //End Page Break

            numErr = 0;
            numStud = 0;

            //Counts the errors
            for (int i = 0; i < Senior.Count; i++)
            {
                numStud++;
                if (Senior[i].returnErr()) { numErr++; }
            }

            //Key and tie error
            if (SKFault) { it.Add("UPPER KEY ERROR     " + SeniorKey); }
            else { it.Add("UPPER KEY ~OK~      " + SeniorKey); }

            if (STFault) { it.Add("UPPER TIE ERROR     " + SeniorTie); }
            else { it.Add("UPPER TIE ~OK~      " + SeniorTie); }

            //Number of students/error students
            it.Add("TOTAL: " + numStud.ToString() + " STUDENTS | ERRORS: " + numErr.ToString() + " STUDENTS");

            //Prints out the error string to the string
            for (int i = 0; i < Senior.Count; i++)
            {
                it.Add(Senior[i].debugString());
            }

            return it;
        }

        public List<string> teamJString()
        {
            bubbleSortJSchool();
            List<string> it = new List<string>();

            it.Add("Lower Division Team Results");
            it.Add(" ");
            it.Add("      Team");
            it.Add("Rank  Score  Ties");
            it.Add("----  -----  ----");

            //The Final Tiebreaker...
            bool flip;
            do
            {
                flip = false;
                for (int i = 0; i < (SchoolList.Count) - 1; i++)
                {
                    if (SchoolList[i].returnJScoreInt() == SchoolList[i + 1].returnJScoreInt() && SchoolList[i].returnJScoreTie() == SchoolList[i + 1].returnJScoreTie())
                    {
                        SchoolList[i].sneakJTie();
                        flip = true;
                    }
                }
            } while (flip);
            //Don't tell anyone it adds based on which schools are first in the file...
            string thing;
            for (int i = 0; i < SchoolList.Count; i++)
            {
                thing = "";
                if (i < 9) { thing = thing + "  "; }
                else { thing = thing + " "; }
                thing = thing + (i + 1).ToString() + ".    ";
                thing = thing + SchoolList[i].returnJScoreInt().ToString() + "   ";
                int thingy = 0;
                while (SchoolList[i].returnJScoreTie().ToString().Length < 4 - thingy)
                {
                    thingy++;
                    thing = thing + " ";
                }

                thing = thing + SchoolList[i].returnJScoreTie().ToString() + "  ";
                thing = thing + SchoolList[i].returnLevel() + " " + SchoolList[i].returnName();

                if (SchoolList[i].returnJScoreInt() != 0)
                {
                    it.Add(thing);
                }
            }

            return it;
        }

        public List<string> teamSString()
        {
            bubbleSortSSchool();
            List<string> it = new List<string>();

            it.Add("Upper Division Team Results");
            it.Add(" ");
            it.Add("      Team");
            it.Add("Rank  Score  Ties");
            it.Add("----  -----  ----");

            //The Final Tiebreaker...
            bool flip;
            do
            {
                flip = false;
                for (int i = 0; i < (SchoolList.Count) - 1; i++)
                {
                    if (SchoolList[i].returnSScoreInt() == SchoolList[i + 1].returnSScoreInt() && SchoolList[i].returnSScoreTie() == SchoolList[i + 1].returnSScoreTie())
                    {
                        SchoolList[i].sneakSTie();
                        flip = true;
                    }
                }
            } while (flip);
            //Don't tell anyone it adds based on which schools are first in the file...
            string thing;
            for (int i = 0; i < SchoolList.Count; i++)
            {
                thing = "";
                if (i < 9) { thing = thing + "  "; }
                else { thing = thing + " "; }
                thing = thing + (i + 1).ToString() + ".    ";
                thing = thing + SchoolList[i].returnSScoreInt().ToString() + "   ";
                int thingy = 0;
                while (SchoolList[i].returnSScoreTie().ToString().Length < 4 - thingy)
                {
                    thingy++;
                    thing = thing + " ";
                }

                thing = thing + SchoolList[i].returnSScoreTie().ToString() + "  ";
                thing = thing + SchoolList[i].returnLevel() + " " + SchoolList[i].returnName();

                if (SchoolList[i].returnSScoreInt() != 0)
                {
                    it.Add(thing);
                }
            }

            return it;
        }

        public void writeToFile(List<string> lines, string fileName, string path)
        {
            TextWriter tw = new StreamWriter(path + fileName + ".txt");
            foreach (string s in lines)
                tw.WriteLine(s);
            tw.Close();
        }

        public List<string> JAwards()
        {
            bubbleSortStudent();
            List<string> it = new List<string>();
            int i = 0;
            while ((i < 10 || Junior[i].returnIntScore() == Junior[i + 1].returnIntScore()) && i < Junior.Count)
            {
                it.Add(Junior[i].awardString());
                i++;
            }
            List<string> that = new List<string>();

            that.Add("LOWER DIVISION INDIVIDUAL AWARDS");
            that.Add("--------------------------------");
            that.Add(" ");
            that.Add("Lower Division Honorable Mantion:");
            that.Add(" ");
            int cntr = it.Count;
            if (cntr >= 10)
            {
                while (cntr > 10)
                {
                    that.Add("       " + it[--cntr]);
                    that.Add(" ");
                }
            }
            else
            {
                that.Add("       none");
            }
            that.Add(" ");
            that.Add("Lower Division Top Ten:");
            that.Add(" ");
            while (cntr > 0)
            {
                if (cntr == 10)
                {
                    that.Add("10 --> " + it[--cntr]);
                }
                else
                {
                    that.Add(" " + cntr.ToString() + " --> " + it[--cntr]);
                }
                that.Add(" ");
            }

            return that;
        }

        public List<string> SAwards()
        {
            bubbleSortStudent();
            List<string> it = new List<string>();
            int i = 0;
            while ((i < 10 || Senior[i].returnIntScore() == Senior[i + 1].returnIntScore()) && i < Senior.Count)
            {
                it.Add(Senior[i].awardString());
                i++;
            }
            List<string> that = new List<string>();

            that.Add("UPPER DIVISION INDIVIDUAL AWARDS");
            that.Add("--------------------------------");
            that.Add(" ");
            that.Add("Upper Division Honorable Mantion:");
            that.Add(" ");
            int cntr = it.Count;
            if (cntr >= 10)
            {
                while (cntr > 10)
                {
                    that.Add("       " + it[--cntr]);
                    that.Add(" ");
                }
            }
            else
            {
                that.Add("       none");
            }
            that.Add(" ");
            that.Add("Upper Division Top Ten:");
            that.Add(" ");
            while (cntr > 0)
            {
                if (cntr == 10)
                {
                    that.Add("10 --> " + it[--cntr]);
                }
                else
                {
                    that.Add(" " + cntr.ToString() + " --> " + it[--cntr]);
                }
                that.Add(" ");
            }

            return that;
        }

        public List<string> DivTeamAwards()
        {
            List<string> it = new List<string>();
            List<string> AA = new List<string>();
            List<string> A = new List<string>();

            it.Add("LOWER DIVISION TEAM AWARDS");
            it.Add("--------------------------");
            it.Add(" ");
            bubbleSortJSchool();
            for (int i = 0; i < SchoolList.Count; i++)
            {
                if (SchoolList[i].returnLevel() == "AA" && AA.Count < 2)
                {
                    AA.Add(SchoolList[i].returnName());
                }
                else if (SchoolList[i].returnLevel() == " A" && A.Count < 4)
                {
                    A.Add(SchoolList[i].returnName());
                }
            }

            for (int i = AA.Count; i > 0; i--)
            {
                it.Add("Class AA - " + (i).ToString() + ": " + AA[i - 1]);
                it.Add(" ");
            }
            it.Add(" ");
            for (int i = A.Count; i > 0; i--)
            {
                it.Add("Class  A - " + (i).ToString() + ": " + A[i - 1]);
                it.Add(" ");
            }
            it.Add(" ");

            AA = new List<string>();
            A = new List<string>();

            it.Add("UPPER DIVISION TEAM AWARDS");
            it.Add("--------------------------");
            it.Add(" ");
            bubbleSortSSchool();
            for (int i = 0; i < SchoolList.Count; i++)
            {
                if (SchoolList[i].returnLevel() == "AA" && AA.Count < 2)
                {
                    AA.Add(SchoolList[i].returnName());
                }
                else if (SchoolList[i].returnLevel() == " A" && A.Count < 4)
                {
                    A.Add(SchoolList[i].returnName());
                }
            }

            for (int i = AA.Count; i > 0; i--)
            {
                it.Add("Class AA - " + (i).ToString() + ": " + AA[i - 1]);
                it.Add(" ");
            }
            it.Add(" ");
            for (int i = A.Count; i > 0; i--)
            {
                it.Add("Class  A - " + (i).ToString() + ": " + A[i - 1]);
                it.Add(" ");
            }

            return it;
        }

        public List<string> JFreqDist()
        {
            List<string> it = new List<string>();

            List<string> dist = new List<string>();

            int totStudents = 0;
            int bigScore = 0;

            it.Add("Lower Division Frequency Distribution");
            it.Add(" ");

            for (int i = 0; i < 41; i++)
            {
                dist.Add("");
            }

            for (int i = 0; i < Junior.Count; i++)
            {
                totStudents++;
                bigScore = bigScore + Junior[i].returnIntScore();
                dist[Junior[i].returnIntScore()] += "*";
            }

            it.Add("Total Scores: " + totStudents.ToString());
            it.Add("Average Score: " + (bigScore / totStudents).ToString());
            it.Add(" ");
            for (int i = 0; i < 41; i++)
            {
                if (i < 10)
                {
                    if (dist[i].Length < 10)
                    {
                        it.Add(" " + i.ToString() + ":   " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                    else
                    {
                        it.Add(" " + i.ToString() + ":  " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                }
                else
                {
                    if (dist[i].Length < 10)
                    {
                        it.Add(i.ToString() + ":   " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                    else
                    {
                        it.Add(i.ToString() + ":  " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                }
            }
            return it;
        }

        public List<string> SFreqDist()
        {
            List<string> it = new List<string>();

            List<string> dist = new List<string>();

            int totStudents = 0;
            int bigScore = 0;

            it.Add("Upper Division Frequency Distribution");
            it.Add(" ");

            for (int i = 0; i < 41; i++)
            {
                dist.Add("");
            }

            for (int i = 0; i < Senior.Count; i++)
            {
                totStudents++;
                bigScore = bigScore + Senior[i].returnIntScore();
                dist[Senior[i].returnIntScore()] += "*";
            }

            it.Add("Total Scores: " + totStudents.ToString());
            it.Add("Average Score: " + (bigScore / totStudents).ToString());
            it.Add(" ");
            for (int i = 0; i < 41; i++)
            {
                if (i < 10)
                {
                    if (dist[i].Length < 10)
                    {
                        it.Add(" " + i.ToString() + ":   " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                    else
                    {
                        it.Add(" " + i.ToString() + ":  " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                }
                else
                {
                    if (dist[i].Length < 10)
                    {
                        it.Add(i.ToString() + ":   " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                    else
                    {
                        it.Add(i.ToString() + ":  " + dist[i].Length.ToString() + " " + dist[i]);
                    }
                }
            }
            return it;
        }

        public List<string> JTestAnalysis()
        {
            List<string> it = new List<string>();

            it.Add("Lower Division Test Item Analysis\n");
            it.Add("Question    NA     1      2      3      4      5     TBR");
            it.Add("--------   ---    ---    ---    ---    ---    ---    ---");

            #region Create Array

            int[,] theNasty = new int[40, 8];
            int tempo;
            string ans;

            for (int i = 0; i < 40; i++)
            {
                theNasty[i, 0] = parseNumHelper(JuniorKey[i]);
                tempo = parseNumHelper(JuniorTie[i]);
                if (tempo <= 0) { theNasty[i, 7] = 0; }
                else { theNasty[i, 7] = tempo; }

                theNasty[i, 1] = 0;
                theNasty[i, 2] = 0;
                theNasty[i, 3] = 0;
                theNasty[i, 4] = 0;
                theNasty[i, 5] = 0;
                theNasty[i, 6] = 0;
            }

            for (int i = 0; i < Junior.Count; i++)
            {
                ans = Junior[i].returnanswers();
                for (int j = 0; j < 40; j++)
                {
                    tempo = parseNumHelper(ans[j]);
                    if (tempo == 1 || tempo == 2 || tempo == 3 || tempo == 4 || tempo == 5) { theNasty[j, tempo]++; }
                    else { theNasty[j, 6]++; }
                }
            }
            #endregion Create Array

            string theLine;
            string c = "*   ";
            string inc = "    ";
            //double percentage;

            for (int i = 0; i < 40; i++)
            {
                theLine = "   ";
                if (i < 9)
                {
                    theLine += " " + (i + 1).ToString() + "      ";
                }
                else
                {
                    theLine += (i + 1).ToString() + "      ";
                }
                //NA
                if (theNasty[i, 6] > 99) { theLine += theNasty[i, 6].ToString() + inc; }
                else if (theNasty[i, 6] > 9) { theLine += " " + theNasty[i, 6].ToString() + inc; }
                else { theLine += "  " + theNasty[i, 6].ToString() + inc; }

                //1
                if (theNasty[i, 1] > 99) { theLine += theNasty[i, 1].ToString(); }
                else if (theNasty[i, 1] > 9) { theLine += " " + theNasty[i, 1].ToString(); }
                else { theLine += "  " + theNasty[i, 1].ToString(); }
                if (theNasty[i, 0] == 1) { theLine += c; }
                else { theLine += inc; }

                //2
                if (theNasty[i, 2] > 99) { theLine += theNasty[i, 2].ToString(); }
                else if (theNasty[i, 2] > 9) { theLine += " " + theNasty[i, 2].ToString(); }
                else { theLine += "  " + theNasty[i, 2].ToString(); }
                if (theNasty[i, 0] == 2) { theLine += c; }
                else { theLine += inc; }

                //3
                if (theNasty[i, 3] > 99) { theLine += theNasty[i, 3].ToString(); }
                else if (theNasty[i, 3] > 9) { theLine += " " + theNasty[i, 3].ToString(); }
                else { theLine += "  " + theNasty[i, 3].ToString(); }
                if (theNasty[i, 0] == 3) { theLine += c; }
                else { theLine += inc; }

                //4
                if (theNasty[i, 4] > 99) { theLine += theNasty[i, 4].ToString(); }
                else if (theNasty[i, 4] > 9) { theLine += " " + theNasty[i, 4].ToString(); }
                else { theLine += "  " + theNasty[i, 4].ToString(); }
                if (theNasty[i, 0] == 4) { theLine += c; }
                else { theLine += inc; }

                //5
                if (theNasty[i, 5] > 99) { theLine += theNasty[i, 5].ToString(); }
                else if (theNasty[i, 5] > 9) { theLine += " " + theNasty[i, 5].ToString(); }
                else { theLine += "  " + theNasty[i, 5].ToString(); }
                if (theNasty[i, 0] == 5) { theLine += c; }
                else { theLine += inc; }

                //TBR
                if (theNasty[i, 7] == 1) { theLine += "  A     "; }
                else if (theNasty[i, 7] == 2) { theLine += "  B     "; }
                else { theLine += "        "; }

                //Percentage
                double top = theNasty[i, theNasty[i, 0]];
                double bottom = (theNasty[i, 1] + theNasty[i, 2] + theNasty[i, 3] + theNasty[i, 4] + theNasty[i, 5]);
                double nummy = ((top / bottom) * 100);
                //ShowPopUpMsg("Top " + top.ToString() + " Bottom " + bottom.ToString() + " nummy " + nummy.ToString());
                theLine += Math.Round(nummy, 1).ToString() + "%";

                it.Add(theLine);
            }


            return it;
        }

        public List<string> STestAnalysis()
        {
            List<string> it = new List<string>();

            it.Add("Upper Division Test Item Analysis\n");
            it.Add("Question    NA     1      2      3      4      5     TBR");
            it.Add("--------   ---    ---    ---    ---    ---    ---    ---");

            #region Create Array

            int[,] theNasty = new int[40, 8];
            int tempo;
            string ans;

            for (int i = 0; i < 40; i++)
            {
                theNasty[i, 0] = parseNumHelper(SeniorKey[i]);
                tempo = parseNumHelper(SeniorTie[i]);
                if (tempo <= 0) { theNasty[i, 7] = 0; }
                else { theNasty[i, 7] = tempo; }

                theNasty[i, 1] = 0;
                theNasty[i, 2] = 0;
                theNasty[i, 3] = 0;
                theNasty[i, 4] = 0;
                theNasty[i, 5] = 0;
                theNasty[i, 6] = 0;
            }

            for (int i = 0; i < Senior.Count; i++)
            {
                ans = Senior[i].returnanswers();
                for (int j = 0; j < 40; j++)
                {
                    tempo = parseNumHelper(ans[j]);
                    if (tempo == 1 || tempo == 2 || tempo == 3 || tempo == 4 || tempo == 5) { theNasty[j, tempo]++; }
                    else { theNasty[j, 6]++; }
                }
            }
            #endregion Create Array

            string theLine;
            string c = "*   ";
            string inc = "    ";
            //double percentage;

            for (int i = 0; i < 40; i++)
            {
                theLine = "   ";
                if (i < 9)
                {
                    theLine += " " + (i + 1).ToString() + "      ";
                }
                else
                {
                    theLine += (i + 1).ToString() + "      ";
                }
                //NA
                if (theNasty[i, 6] > 99) { theLine += theNasty[i, 6].ToString() + inc; }
                else if (theNasty[i, 6] > 9) { theLine += " " + theNasty[i, 6].ToString() + inc; }
                else { theLine += "  " + theNasty[i, 6].ToString() + inc; }

                //1
                if (theNasty[i, 1] > 99) { theLine += theNasty[i, 1].ToString(); }
                else if (theNasty[i, 1] > 9) { theLine += " " + theNasty[i, 1].ToString(); }
                else { theLine += "  " + theNasty[i, 1].ToString(); }
                if (theNasty[i, 0] == 1) { theLine += c; }
                else { theLine += inc; }

                //2
                if (theNasty[i, 2] > 99) { theLine += theNasty[i, 2].ToString(); }
                else if (theNasty[i, 2] > 9) { theLine += " " + theNasty[i, 2].ToString(); }
                else { theLine += "  " + theNasty[i, 2].ToString(); }
                if (theNasty[i, 0] == 2) { theLine += c; }
                else { theLine += inc; }

                //3
                if (theNasty[i, 3] > 99) { theLine += theNasty[i, 3].ToString(); }
                else if (theNasty[i, 3] > 9) { theLine += " " + theNasty[i, 3].ToString(); }
                else { theLine += "  " + theNasty[i, 3].ToString(); }
                if (theNasty[i, 0] == 3) { theLine += c; }
                else { theLine += inc; }

                //4
                if (theNasty[i, 4] > 99) { theLine += theNasty[i, 4].ToString(); }
                else if (theNasty[i, 4] > 9) { theLine += " " + theNasty[i, 4].ToString(); }
                else { theLine += "  " + theNasty[i, 4].ToString(); }
                if (theNasty[i, 0] == 4) { theLine += c; }
                else { theLine += inc; }

                //5
                if (theNasty[i, 5] > 99) { theLine += theNasty[i, 5].ToString(); }
                else if (theNasty[i, 5] > 9) { theLine += " " + theNasty[i, 5].ToString(); }
                else { theLine += "  " + theNasty[i, 5].ToString(); }
                if (theNasty[i, 0] == 5) { theLine += c; }
                else { theLine += inc; }

                //TBR
                if (theNasty[i, 7] == 1) { theLine += "  A     "; }
                else if (theNasty[i, 7] == 2) { theLine += "  B     "; }
                else { theLine += "        "; }

                //Percentage
                double top = theNasty[i, theNasty[i, 0]];
                double bottom = (theNasty[i, 1] + theNasty[i, 2] + theNasty[i, 3] + theNasty[i, 4] + theNasty[i, 5]);
                double nummy = ((top / bottom) * 100);
                //ShowPopUpMsg("Top " + top.ToString() + " Bottom " + bottom.ToString() + " nummy " + nummy.ToString());
                theLine += Math.Round(nummy, 1).ToString() + "%";

                it.Add(theLine);
            }


            return it;
        }

        public List<string> rankJ()
        {
            List<string> it = new List<string>();
            //bubbleSortStudent();

            it.Add("Lower Division Coded Scores With Tiebreakers\n");
            it.Add("Key:  " + JuniorKey);
            it.Add("Ties: " + JuniorTie + "\n");

            string line;
            for (int i = 0; i < Junior.Count; i++)
            {
                line = "";
                line += Junior[i].returnLongScore() + " ";
                line += Junior[i].returnName() + " ";
                line += Junior[i].returngrade() + " " + Junior[i].returnLevel() + " ";
                line += Junior[i].returnSchoolName() + " ";
                line += Junior[i].returnanswers();
                it.Add(line);
            }
            return it;
        }

        public List<string> rankS()
        {
            List<string> it = new List<string>();
            //bubbleSortStudent();

            it.Add("Upper Division Coded Scores With Tiebreakers\n");
            it.Add("Key:  " + SeniorKey);
            it.Add("Ties: " + SeniorTie + "\n");

            string line;
            for (int i = 0; i < Junior.Count; i++)
            {
                line = "";
                line += Senior[i].returnLongScore() + " ";
                line += Senior[i].returnName() + " ";
                line += Senior[i].returngrade() + " " + Senior[i].returnLevel() + " ";
                line += Senior[i].returnSchoolName() + " ";
                line += Senior[i].returnanswers();
                it.Add(line);
            }
            return it;
        }

        public List<string> CreateHeader()
        {
            List<string> it = new List<string>();
            it.Add("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            it.Add("           " + getAnnualPostFix() + " Annual Northern Minnesota Mathematics Contest");
            it.Add("                           " + theDate);
            it.Add(" ");
            it.Add("             Department of Mathematics and Computer Science");
            it.Add("                         Bemidji State University");
            return it;
        }

        public void masterOutput()
        {
            List<string> header = CreateHeader();
            List<string> JTest = JTestAnalysis();
            List<string> JFreq = JFreqDist();
            List<string> JTeam = teamJString();
            List<string> STest = STestAnalysis();
            List<string> SFreq = SFreqDist();
            List<string> STeam = teamSString();
            List<string> JRank = rankJ();
            List<string> SRank = rankS();

            instructorOutput = new List<string>();

            instructorOutput.Add("INSTRUCTOR INFORMATION");
            //Header
            for (int j = 0; j < header.Count; j++)
            {
                instructorOutput.Add(header[j]);
            }
            //*
            instructorOutput.Add("\f");
            //JTest
            for (int j = 0; j < JTest.Count; j++)
            {
                instructorOutput.Add(JTest[j]);
            }//*/

            instructorOutput.Add("\f");
            //JFreq
            for (int j = 0; j < JFreq.Count; j++)
            {
                instructorOutput.Add(JFreq[j]);
            }

            instructorOutput.Add("\f");
            //JTeam
            for (int j = 0; j < JTeam.Count; j++)
            {
                instructorOutput.Add(JTeam[j]);
            }
            //*
            instructorOutput.Add("\f");
            //STest
            for (int j = 0; j < STest.Count; j++)
            {
                instructorOutput.Add(STest[j]);
            }//*/

            instructorOutput.Add("\f");
            //SFreq
            for (int j = 0; j < SFreq.Count; j++)
            {
                instructorOutput.Add(SFreq[j]);
            }

            instructorOutput.Add("\f");
            //STeam
            for (int j = 0; j < STeam.Count; j++)
            {
                instructorOutput.Add(STeam[j]);
            }
            //*
            instructorOutput.Add("\f");
            //JRank
            for (int j = 0; j < JRank.Count; j++)
            {
                instructorOutput.Add(JRank[j]);
            }//*/
            //*
            instructorOutput.Add("\f");
            //SRank
            for (int j = 0; j < SRank.Count; j++)
            {
                instructorOutput.Add(SRank[j]);
            }//*/
        }

        public void SchoolOutputStrings()
        {
            List<string> header = CreateHeader();
            //Individual Schools Go Here In The Final Output.
            List<string> JAward = JAwards();
            List<string> SAward = SAwards();
            List<string> teamAwards = DivTeamAwards();
            List<string> JFreq = JFreqDist();
            List<string> JTeam = teamJString();
            List<string> SFreq = SFreqDist();
            List<string> STeam = teamSString();

            List<string> LastBit = new List<string>();

            #region Create Last Bit
            for (int i = 0; i < JAward.Count; i++)
            {
                LastBit.Add(JAward[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < SAward.Count; i++)
            {
                LastBit.Add(SAward[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < teamAwards.Count; i++)
            {
                LastBit.Add(teamAwards[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < JFreq.Count; i++)
            {
                LastBit.Add(JFreq[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < JTeam.Count; i++)
            {
                LastBit.Add(JTeam[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < SFreq.Count; i++)
            {
                LastBit.Add(SFreq[i]);
            }

            LastBit.Add("\f");

            for (int i = 0; i < STeam.Count; i++)
            {
                LastBit.Add(STeam[i]);
            }

            #endregion Create Last Bit

            List<string> it;

            List<string> jOutput;
            List<string> sOutput;

            for (int i = 0; i < SchoolList.Count; i++)
            {
                it = new List<string>();

                jOutput = SchoolList[i].formatJOutput();
                sOutput = SchoolList[i].formatSOutput();

                it.Add(SchoolList[i].returnName());

                for (int j = 0; j < header.Count; j++)
                {
                    it.Add(header[j]);
                }

                it.Add("\f");

                for (int j = 0; j < jOutput.Count; j++)
                {
                    it.Add(jOutput[j]);
                }

                it.Add("\f");

                for (int j = 0; j < sOutput.Count; j++)
                {
                    it.Add(sOutput[j]);
                }

                it.Add("\f");

                for (int j = 0; j < LastBit.Count; j++)
                {
                    it.Add(LastBit[j]);
                }

                SchoolList[i].setOutput(it);
            }

        }
        
        #endregion String Builder Functions

        #endregion Helper Functions

    }
}
