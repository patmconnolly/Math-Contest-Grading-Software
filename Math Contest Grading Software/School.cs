using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Contest_Grading_Software
{
    public class School
    {
        #region Variable Defanitions

        //Info about school
        int schoolCode;
        string schoolName;
        bool used;
        string level;

        //List of students
        List<Student> Juniors;
        List<Student> Seniors;

        double JScore;
        int JScoreInt;
        int JScoreTie;

        double SScore;
        int SScoreInt;
        int SScoreTie;

        List<string> OutputFile;

        #endregion Variable Defanitions

        #region Constructor / Destructor

        public School(List<string> it)
        {
            used = false;
            schoolCode = parseNumber(it[0]);
            schoolName = it[1];
            for (int i = 2; i < it.Count; i++)
            {
                schoolName = schoolName + " " + it[i];
            }

            Juniors = new List<Student>();
            Seniors = new List<Student>();
        }

        ~School()
        {
            //Intentionally left blank
        }

        #endregion Constructor / Destructor

        #region Setters

        public void setOutput(List<string> it)
        {
            OutputFile = it;
        }

        public void addJunior(Student it)
        {
            Juniors.Add(it);
            used = true;
        }

        public void addSenior(Student it)
        {
            Seniors.Add(it);
            used = true;
        }

        public void sneakJTie()
        {
            JScoreTie += 1;
        }

        public void sneakSTie()
        {
            SScoreTie += 1;
        }

        #endregion Setters

        #region Getters

        public List<string> getOutput()
        {
            return OutputFile;
        }

        public int returnJScoreInt()
        {
            return JScoreInt;
        }

        public int returnJScoreTie()
        {
            return JScoreTie;
        }

        public int returnSScoreInt()
        {
            return SScoreInt;
        }

        public int returnSScoreTie()
        {
            return SScoreTie;
        }

        public bool isUsed()
        {
            return used;
        }

        public int JCount()
        {
            return Juniors.Count;
        }

        public int SCount()
        {
            return Seniors.Count;
        }

        public Student returnJunior(int i)
        {
            return Juniors[i];
        }

        public Student returnSenior(int i)
        {
            return Seniors[i];
        }

        public int returnCode()
        {
            return schoolCode;
        }

        public string returnName()
        {
            return schoolName;
        }

        public double returnJScore()
        {
            return JScore;
        }

        public int returnIntJScore()
        {
            return (int)JScore;
        }

        public double returnSScore()
        {
            return SScore;
        }

        public int returnIntSScore()
        {
            return (int)SScore;
        }

        public string returnLevel()
        {
            return level;
        }

        #endregion Getters

        #region Parse Functions

        private int parseNumber(string it)
        {
            int theNumber = 0;
            for (int i = 0; i < it.Length; i++)
            {
                theNumber = (theNumber * 10) + parseNumHelper(it[i]);
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

        private void bubbleSort()
        {
            Student temp;

            for (int i = Juniors.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (Juniors[j].returnscore() < Juniors[j + 1].returnscore())
                    {
                        temp = Juniors[j];
                        Juniors[j] = Juniors[j + 1];
                        Juniors[j + 1] = temp;
                    }
                }
            }

            for (int i = Seniors.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (Seniors[j].returnscore() < Seniors[j + 1].returnscore())
                    {
                        temp = Seniors[j];
                        Seniors[j] = Seniors[j + 1];
                        Seniors[j + 1] = temp;
                    }
                }
            }
        }

        public void calcTeamScore()
        {
            figureOutLevel();
            bubbleSort();

            int counter = 0;
            JScore = 0;
            SScore = 0;

            while (counter < 3)
            {
                if (counter < Juniors.Count) { JScore = JScore + Juniors[counter].returnscore(); }
                if (counter < Seniors.Count) { SScore = SScore + Seniors[counter].returnscore(); }
                counter++;
            }

            JScoreInt = (int)JScore;
            SScoreInt = (int)SScore;
            JScoreTie = (int)((JScore - JScoreInt) * 10000);
            SScoreTie = (int)((SScore - SScoreInt) * 10000);
        }

        private void figureOutLevel()
        {
            int temp = 0;
            for (int i = 0; i < Juniors.Count; i++)
            {
                temp = temp + Juniors[i].returnLevelCode() % 10;
            }

            for (int i = 0; i < Seniors.Count; i++)
            {
                temp = temp + Seniors[i].returnLevelCode() % 10;
            }

            temp = temp / (Juniors.Count + Seniors.Count);

            if (temp < 5) { level = "AA"; }
            else { level = " A"; }
        }

        #endregion Parse Functions

        #region Formatted Output

        public string returnJTeamString()
        {
            return JScore.ToString().PadLeft(8) + level.PadLeft(3).PadRight(4) + schoolName;
        }

        public string returnSTeamString()
        {
            return SScore.ToString().PadLeft(8) + level.PadLeft(3).PadRight(4) + schoolName;
        }

        public List<string> formatJOutput()
        {
            List<string> it = new List<string>();
            bubbleSort();

            //Pre People Text

            it.Add(schoolName + "    Lower    " + level);
            it.Add(" ");

            //End Pre Text

            string score;

            string line;

            for (int i = 0; i < Juniors.Count; i++)
            {
                score = Juniors[i].returnIntScore().ToString();
                if (score.Length < 4)
                {
                    score = score + " ";
                }
                line = score;
                if (i < 3)
                {
                    line = line + "  * ";
                }
                else
                {
                    line = line + "    ";
                }
                if (Juniors[i].returnIntScore() < 10)
                {
                    line = line + " " + Juniors[i].returnName();
                }
                else
                {
                    line = line + Juniors[i].returnName();
                }

                it.Add(line);
            }

            //Post People Text
            it.Add(" ");
            it.Add("Team Score: " + ((int)JScore).ToString());
            it.Add(" ");
            it.Add("* Team members awarded a T-shirt.");
            it.Add(" ");
            it.Add(Juniors.Count.ToString() + " students participated.");

            return it;
        }

        public List<string> formatSOutput()
        {
            List<string> it = new List<string>();
            bubbleSort();

            //Pre People Text

            it.Add(schoolName + "    Upper    " + level);
            it.Add(" ");

            //End Pre Text

            string score;

            string line;

            for (int i = 0; i < Seniors.Count; i++)
            {
                score = Seniors[i].returnIntScore().ToString();
                if (score.Length < 4)
                {
                    score = score + " ";
                }
                line = score;
                if (i < 3)
                {
                    line = line + "  * ";
                }
                else
                {
                    line = line + "    ";
                }
                if (Seniors[i].returnIntScore() < 10)
                {
                    line = line + " " + Seniors[i].returnName();
                }
                else
                {
                    line = line + Seniors[i].returnName();
                }
                it.Add(line);
            }
            //Post People Text
            it.Add(" ");
            it.Add("Team Score: " + ((int)SScore).ToString());
            it.Add(" ");
            it.Add("* Team members awarded a T-shirt.");
            it.Add(" ");
            it.Add(Seniors.Count.ToString() + " students participated.");

            return it;
        }

        #endregion Formatted Output
    }
}
