namespace Cellm.AddIn.UserInterface.Forms;

partial class ProfileForm
{
    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.Label systemPromptLabel;
    private System.Windows.Forms.TextBox systemPromptTextBox;
    private System.Windows.Forms.Label temperatureLabel;
    private System.Windows.Forms.NumericUpDown temperatureNumeric;
    private System.Windows.Forms.Label maxOutputTokensLabel;
    private System.Windows.Forms.NumericUpDown maxOutputTokensNumeric;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button deleteButton;

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        nameLabel = new Label();
        nameTextBox = new TextBox();
        systemPromptLabel = new Label();
        systemPromptTextBox = new TextBox();
        temperatureLabel = new Label();
        temperatureNumeric = new NumericUpDown();
        maxOutputTokensLabel = new Label();
        maxOutputTokensNumeric = new NumericUpDown();
        okButton = new Button();
        cancelButton = new Button();
        deleteButton = new Button();
        ((System.ComponentModel.ISupportInitialize)temperatureNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)maxOutputTokensNumeric).BeginInit();
        SuspendLayout();
        //
        // nameLabel
        //
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(14, 17);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(42, 15);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "Name:";
        //
        // nameTextBox
        //
        nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        nameTextBox.Location = new Point(120, 14);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(318, 23);
        nameTextBox.TabIndex = 0;
        //
        // systemPromptLabel
        //
        systemPromptLabel.AutoSize = true;
        systemPromptLabel.Location = new Point(14, 47);
        systemPromptLabel.Name = "systemPromptLabel";
        systemPromptLabel.Size = new Size(90, 15);
        systemPromptLabel.TabIndex = 2;
        systemPromptLabel.Text = "System Prompt:";
        //
        // systemPromptTextBox
        //
        systemPromptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        systemPromptTextBox.Location = new Point(120, 44);
        systemPromptTextBox.Multiline = true;
        systemPromptTextBox.Name = "systemPromptTextBox";
        systemPromptTextBox.ScrollBars = ScrollBars.Vertical;
        systemPromptTextBox.Size = new Size(318, 120);
        systemPromptTextBox.TabIndex = 1;
        //
        // temperatureLabel
        //
        temperatureLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        temperatureLabel.AutoSize = true;
        temperatureLabel.Location = new Point(14, 177);
        temperatureLabel.Name = "temperatureLabel";
        temperatureLabel.Size = new Size(79, 15);
        temperatureLabel.TabIndex = 4;
        temperatureLabel.Text = "Temperature:";
        //
        // temperatureNumeric
        //
        temperatureNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        temperatureNumeric.DecimalPlaces = 1;
        temperatureNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
        temperatureNumeric.Location = new Point(120, 175);
        temperatureNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        temperatureNumeric.Name = "temperatureNumeric";
        temperatureNumeric.Size = new Size(80, 23);
        temperatureNumeric.TabIndex = 2;
        //
        // maxOutputTokensLabel
        //
        maxOutputTokensLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        maxOutputTokensLabel.AutoSize = true;
        maxOutputTokensLabel.Location = new Point(14, 207);
        maxOutputTokensLabel.Name = "maxOutputTokensLabel";
        maxOutputTokensLabel.Size = new Size(100, 15);
        maxOutputTokensLabel.TabIndex = 6;
        maxOutputTokensLabel.Text = "Max Output Tokens:";
        //
        // maxOutputTokensNumeric
        //
        maxOutputTokensNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        maxOutputTokensNumeric.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        maxOutputTokensNumeric.Location = new Point(120, 205);
        maxOutputTokensNumeric.Maximum = new decimal(new int[] { 128000, 0, 0, 0 });
        maxOutputTokensNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        maxOutputTokensNumeric.Name = "maxOutputTokensNumeric";
        maxOutputTokensNumeric.Size = new Size(80, 23);
        maxOutputTokensNumeric.TabIndex = 3;
        maxOutputTokensNumeric.Value = new decimal(new int[] { 8192, 0, 0, 0 });
        //
        // deleteButton
        //
        deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        deleteButton.Location = new Point(14, 245);
        deleteButton.Name = "deleteButton";
        deleteButton.Size = new Size(88, 27);
        deleteButton.TabIndex = 6;
        deleteButton.Text = "Delete";
        deleteButton.UseVisualStyleBackColor = true;
        deleteButton.Click += deleteButton_Click;
        //
        // okButton
        //
        okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        okButton.DialogResult = DialogResult.OK;
        okButton.Location = new Point(244, 245);
        okButton.Name = "okButton";
        okButton.Size = new Size(88, 27);
        okButton.TabIndex = 4;
        okButton.Text = "OK";
        okButton.UseVisualStyleBackColor = true;
        okButton.Click += okButton_Click;
        //
        // cancelButton
        //
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(338, 245);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(88, 27);
        cancelButton.TabIndex = 5;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        //
        // ProfileForm
        //
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(452, 285);
        Controls.Add(deleteButton);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        Controls.Add(maxOutputTokensNumeric);
        Controls.Add(maxOutputTokensLabel);
        Controls.Add(temperatureNumeric);
        Controls.Add(temperatureLabel);
        Controls.Add(systemPromptTextBox);
        Controls.Add(systemPromptLabel);
        Controls.Add(nameTextBox);
        Controls.Add(nameLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ProfileForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Profile";
        ((System.ComponentModel.ISupportInitialize)temperatureNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)maxOutputTokensNumeric).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion
}
