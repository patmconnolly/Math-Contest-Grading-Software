using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Contest_Grading_Software
{
    public class Student
    {

        #region Variable Definitions
        List<string> theLine;     //Basic entry line, parse this

        string Name;        //Last, First MI

        int LevelCode;   //41, 49, 51, 59
        string grade;       //Upper or Lower (Senior or Junior)
        string Level;       //AA or A

        int schoolCode;     //Reduced to integar
        string schoolName;  //String name of school

        string answers;     //Answer string

        double score;       //Int val is raw score, decimal is tiebreakers. Format is 00.1122

        #endregion Variable Definitions

        #region Boolian

        bool ansErr = false;
        bool scErr = false;
        bool levErr = false;


        #endregion Boolian

        #region Constructor/Destructor

        public Student(List<string> it, List<int> schNum)
        {
            theLine = it;
            //Do Stuff
            parse();
            validate();

            updateInfo();
        }

        ~Student()
        {
            //Intentionally left blank
        }

        #endregion Constructor/Destructor

        #region Setters
        public void setName(string it)
        {
            Name = it;
        }

        public void setLevelCode(int it)
        {
            LevelCode = it;
        }
        public void setgrade(string it)
        {
            grade = it;
        }
        public void setLevel(string it)
        {
            Level = it;
        }

        public void setschoolCode(int it)
        {
            schoolCode = it;
        }
        public void setSchoolName(string it)
        {
            schoolName = it;
        }

        public void setanswers(string it)
        {
            answers = it;
        }

        public void setscore(double it)
        {
            score = it;
        }

        public void setSchoolNameError(bool it)
        {
            scErr = it;
        }
        #endregion Setters

        #region Getters

        public string returnName()        //Last, First MI
        {
            return Name;
        }

        public int returnLevelCode()   //41, 49, 51, 59
        {
            return LevelCode;
        }

        public string returngrade()       //Junior or Senior
        {
            return grade;
        }

        public string returnLevel()       //AA or A
        {
            return Level;
        }

        public int returnschoolCode()     //Reduced to integar
        {
            return schoolCode;
        }

        public string returnSchoolName()
        {
            return schoolName;
        }

        public string returnanswers()     //Answer string
        {
            return answers;
        }

        public double returnscore()       //Int val is raw score, decimal is tiebreakers. Format is 00.1122
        {
            return score;
        }

        public int returnIntScore()
        {
            return (int)score;
        }

        public bool returnErr()
        {
            return (ansErr || scErr || levErr);
        }

        public int returnLongScore()
        {
            return (int)(score * 10000);
        }

        #endregion Getters

        #region Parse Functions

        private void parse()
        {
            int counter = theLine.Count;
            if (counter > 0)
            {
                answers = theLine[--counter];                   //Gets the answers
            }
            else
            {
                ansErr = true;
                answers = "~~LINE EMPTY ERROR, MANUALLY ENTER KEY~~";
            }
            if (counter > 0)
            {
                schoolCode = parseNumber(theLine[--counter]);   //Gets the school Code
            }
            else { scErr = true; }
            if (counter > 0)
            {
                LevelCode = parseNumber(theLine[--counter]);    //Gets the Level Code
            }
            else { levErr = true; }

            Name = "";                                      //Gets the name and formats it
            if (counter >= 0)
            {
                while (counter > 1)
                {
                    Name = theLine[--counter] + " " + Name;
                }
                Name = theLine[0] + ", " + Name;
            }
        }

        private void validate()
        {
            //Validates answer string
            if (answers.Length != 40)
            {
                ansErr = true;
                answers = "~~LENGTH ERROR~~  ~~~~  ~~LENGTH ERROR~~";
            }
            else
            {
                for (int i = 0; i < 40; i++)
                {
                    if (answers[i] != '1' && answers[i] != '2' && answers[i] != '3' && answers[i] != '4' && answers[i] != '5' && answers[i] != '6' && answers[i] != '7' && answers[i] != '8' && answers[i] != '9' && answers[i] != '0' && answers[i] != '*')
                    {
                        ansErr = true;
                        answers = "~~~KEY ERROR~~~KEY  ERROR~~~KEY ERROR~~~";
                    }
                }
            }

            //Validates the school code
            if (schoolCode == -1)
            {
                scErr = true;
            }

            //Validates Level Code
            if (LevelCode != 41 && LevelCode != 49 && LevelCode != 51 && LevelCode != 59)
            {
                levErr = true;
            }
        }

        private void updateInfo()       //Junior/Senior - AA/A
        {
            //int LevelCode;   //41, 49, 51, 59
            //string grade;       //Junior or Senior
            //string Level;       //AA or A
            if (LevelCode == 41 || LevelCode == 51) { Level = "AA"; }
            else if (LevelCode == 49 || LevelCode == 59) { Level = " A"; }
            else { Level = "ER"; }

            if (LevelCode == 41 || LevelCode == 49) { grade = "Lower"; }
            else if (LevelCode == 51 || LevelCode == 59) { grade = "Upper"; }
            else { grade = " ERROR"; }
        }

        #endregion Parse Functions

        #region Odd Functions

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

        #endregion Odd Functions

        #region String Builder

        public string debugString()
        {
            string it;
            string NaMe;
            string GrAdE;
            string LvL;
            string ScH;

            if (returnErr())
            {
                it = "ERROR ";
            }
            else
            {
                it = "~OK~  ";
            }

            NaMe = returnName();
            while (NaMe.Length <= 40)
            {
                NaMe = NaMe + " ";
            }

            //Division (Upper/Lower) : 8chars
            GrAdE = returngrade();
            while (GrAdE.Length <= 8)
            {
                GrAdE = GrAdE + " ";
            }

            //Level (AA/A) : 5chars
            LvL = returnLevel();
            while (LvL.Length <= 5)
            {
                LvL = LvL + " ";
            }

            //School Name : 30chars
            ScH = returnSchoolName();
            while (ScH.Length <= 30)
            {
                ScH = ScH + " ";
            }

            //Put it all together with the answer key at the end
            it = it + NaMe + GrAdE + LvL + ScH + returnanswers();

            return it;
        }

        public string awardString()
        {
            string it = "";
            it = it + Name + ": " + schoolName;
            return it;
        }

        #endregion String Builder
    }
}
