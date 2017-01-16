namespace Math_Contest_Grading_Software
{
    partial class MCG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCG));
            this.validateGrade = new System.Windows.Forms.Button();
            this.lowerFile = new System.Windows.Forms.Button();
            this.upperFile = new System.Windows.Forms.Button();
            this.schoolFile = new System.Windows.Forms.Button();
            this.LFileBox = new System.Windows.Forms.TextBox();
            this.UFileBox = new System.Windows.Forms.TextBox();
            this.SFileBox = new System.Windows.Forms.TextBox();
            this.month = new System.Windows.Forms.ComboBox();
            this.day = new System.Windows.Forms.ComboBox();
            this.year = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // validateGrade
            // 
            this.validateGrade.Location = new System.Drawing.Point(12, 101);
            this.validateGrade.Name = "validateGrade";
            this.validateGrade.Size = new System.Drawing.Size(257, 23);
            this.validateGrade.TabIndex = 0;
            this.validateGrade.Text = "Validate / Grade";
            this.validateGrade.UseVisualStyleBackColor = true;
            this.validateGrade.Click += new System.EventHandler(this.validateGrade_Click);
            // 
            // lowerFile
            // 
            this.lowerFile.Location = new System.Drawing.Point(12, 12);
            this.lowerFile.Name = "lowerFile";
            this.lowerFile.Size = new System.Drawing.Size(75, 23);
            this.lowerFile.TabIndex = 2;
            this.lowerFile.Text = "Lower File";
            this.lowerFile.UseVisualStyleBackColor = true;
            this.lowerFile.Click += new System.EventHandler(this.lowerFile_Click);
            // 
            // upperFile
            // 
            this.upperFile.Location = new System.Drawing.Point(12, 42);
            this.upperFile.Name = "upperFile";
            this.upperFile.Size = new System.Drawing.Size(75, 23);
            this.upperFile.TabIndex = 3;
            this.upperFile.Text = "Upper File";
            this.upperFile.UseVisualStyleBackColor = true;
            this.upperFile.Click += new System.EventHandler(this.upperFile_Click);
            // 
            // schoolFile
            // 
            this.schoolFile.Location = new System.Drawing.Point(12, 72);
            this.schoolFile.Name = "schoolFile";
            this.schoolFile.Size = new System.Drawing.Size(75, 23);
            this.schoolFile.TabIndex = 4;
            this.schoolFile.Text = "School File";
            this.schoolFile.UseVisualStyleBackColor = true;
            this.schoolFile.Click += new System.EventHandler(this.schoolFile_Click);
            // 
            // LFileBox
            // 
            this.LFileBox.Enabled = false;
            this.LFileBox.Location = new System.Drawing.Point(94, 13);
            this.LFileBox.Name = "LFileBox";
            this.LFileBox.Size = new System.Drawing.Size(175, 20);
            this.LFileBox.TabIndex = 5;
            this.LFileBox.Text = "Please Select Lower Division File...";
            // 
            // UFileBox
            // 
            this.UFileBox.Enabled = false;
            this.UFileBox.Location = new System.Drawing.Point(93, 42);
            this.UFileBox.Name = "UFileBox";
            this.UFileBox.Size = new System.Drawing.Size(176, 20);
            this.UFileBox.TabIndex = 6;
            this.UFileBox.Text = "Please Select Upper Division File...";
            // 
            // SFileBox
            // 
            this.SFileBox.Enabled = false;
            this.SFileBox.Location = new System.Drawing.Point(94, 72);
            this.SFileBox.Name = "SFileBox";
            this.SFileBox.Size = new System.Drawing.Size(175, 20);
            this.SFileBox.TabIndex = 7;
            this.SFileBox.Text = "Please Select School File...";
            // 
            // month
            // 
            this.month.FormattingEnabled = true;
            this.month.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.month.Location = new System.Drawing.Point(13, 147);
            this.month.MaxDropDownItems = 12;
            this.month.Name = "month";
            this.month.Size = new System.Drawing.Size(121, 21);
            this.month.TabIndex = 8;
            // 
            // day
            // 
            this.day.FormattingEnabled = true;
            this.day.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.day.Location = new System.Drawing.Point(141, 147);
            this.day.MaxDropDownItems = 31;
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(54, 21);
            this.day.TabIndex = 9;
            // 
            // year
            // 
            this.year.FormattingEnabled = true;
            this.year.Items.AddRange(new object[] {
            "2016",
            "2017",
            "2018",
            "2019",
            "2020",
            "2021",
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030",
            "2031",
            "2032",
            "2033",
            "2034",
            "2035",
            "2036",
            "2037",
            "2038",
            "2039",
            "2040",
            "2041",
            "2042",
            "2043",
            "2044",
            "2045",
            "2046",
            "2047",
            "2048",
            "2049",
            "2050"});
            this.year.Location = new System.Drawing.Point(201, 147);
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(68, 21);
            this.year.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Month";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Day";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Year";
            // 
            // MCG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 180);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.year);
            this.Controls.Add(this.day);
            this.Controls.Add(this.month);
            this.Controls.Add(this.SFileBox);
            this.Controls.Add(this.UFileBox);
            this.Controls.Add(this.LFileBox);
            this.Controls.Add(this.schoolFile);
            this.Controls.Add(this.upperFile);
            this.Controls.Add(this.lowerFile);
            this.Controls.Add(this.validateGrade);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MCG";
            this.Text = "Math Contest Grading Software";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button validateGrade;
        private System.Windows.Forms.Button lowerFile;
        private System.Windows.Forms.Button upperFile;
        private System.Windows.Forms.Button schoolFile;
        private System.Windows.Forms.TextBox LFileBox;
        private System.Windows.Forms.TextBox UFileBox;
        private System.Windows.Forms.TextBox SFileBox;
        private System.Windows.Forms.ComboBox month;
        private System.Windows.Forms.ComboBox day;
        private System.Windows.Forms.ComboBox year;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

