namespace MyNote
{
    partial class FormEvent
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Work");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Life");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Learn");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Play");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Total", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewRefList = new System.Windows.Forms.DataGridView();
            this.DataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewList = new System.Windows.Forms.DataGridView();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRefList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRefList
            // 
            this.dataGridViewRefList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRefList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRefList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumn9});
            this.dataGridViewRefList.Location = new System.Drawing.Point(483, 34);
            this.dataGridViewRefList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewRefList.MultiSelect = false;
            this.dataGridViewRefList.Name = "dataGridViewRefList";
            this.dataGridViewRefList.RowHeadersWidth = 62;
            this.dataGridViewRefList.RowTemplate.Height = 24;
            this.dataGridViewRefList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRefList.Size = new System.Drawing.Size(1130, 1105);
            this.dataGridViewRefList.TabIndex = 2;
            this.dataGridViewRefList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRefList_CellEndEdit);
            this.dataGridViewRefList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewRefList_ColumnHeaderMouseClick);
            this.dataGridViewRefList.SelectionChanged += new System.EventHandler(this.dataGridViewRefList_SelectionChanged);
            // 
            // DataGridViewTextBoxColumn9
            // 
            this.DataGridViewTextBoxColumn9.HeaderText = "RefName";
            this.DataGridViewTextBoxColumn9.MinimumWidth = 8;
            this.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9";
            this.DataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 207);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "ノード1";
            treeNode1.Text = "Work";
            treeNode2.Name = "ノード2";
            treeNode2.Text = "Life";
            treeNode3.Name = "ノード3";
            treeNode3.Text = "Learn";
            treeNode4.Name = "ノード4";
            treeNode4.Text = "Play";
            treeNode5.Name = "ノード0";
            treeNode5.Text = "Total";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.treeView1.Size = new System.Drawing.Size(465, 932);
            this.treeView1.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 34);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(194, 37);
            this.dateTimePicker1.TabIndex = 4;
            this.dateTimePicker1.Value = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dataGridViewList
            // 
            this.dataGridViewList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CategoryName,
            this.ParentCategory});
            this.dataGridViewList.Location = new System.Drawing.Point(1629, 34);
            this.dataGridViewList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewList.Name = "dataGridViewList";
            this.dataGridViewList.RowHeadersWidth = 62;
            this.dataGridViewList.RowTemplate.Height = 24;
            this.dataGridViewList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewList.Size = new System.Drawing.Size(420, 1105);
            this.dataGridViewList.TabIndex = 13;
            // 
            // CategoryName
            // 
            this.CategoryName.HeaderText = "Name";
            this.CategoryName.MinimumWidth = 8;
            this.CategoryName.Name = "CategoryName";
            // 
            // ParentCategory
            // 
            this.ParentCategory.HeaderText = "Parent";
            this.ParentCategory.MinimumWidth = 8;
            this.ParentCategory.Name = "ParentCategory";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(357, 34);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(120, 40);
            this.buttonUpdate.TabIndex = 20;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 18);
            this.label8.TabIndex = 22;
            this.label8.Text = "Start";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(91, 87);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(157, 27);
            this.dateTimePicker2.TabIndex = 23;
            this.dateTimePicker2.Value = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker3.Location = new System.Drawing.Point(91, 123);
            this.dateTimePicker3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(157, 27);
            this.dateTimePicker3.TabIndex = 24;
            this.dateTimePicker3.Value = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 25;
            this.label1.Text = "End";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 26;
            this.button1.Text = "Qurey";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(231, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 40);
            this.button2.TabIndex = 27;
            this.button2.Text = "AllowChange";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(254, 158);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 30);
            this.button3.TabIndex = 28;
            this.button3.Text = "Chart";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(91, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 28);
            this.textBox1.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "Title";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 987);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(2372, 394);
            this.chart1.TabIndex = 31;
            this.chart1.Text = "chart1";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(360, 160);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 30);
            this.button4.TabIndex = 32;
            this.button4.Text = "ChartUp";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FormEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2698, 1474);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.dataGridViewList);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.dataGridViewRefList);
            this.Name = "FormEvent";
            this.Text = "FormEvent";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEvent_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRefList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRefList;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn9;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dataGridViewList;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCategory;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button4;
    }
}