namespace Cellm.AddIn.UserInterface.Forms;

public partial class ProfileForm : Form
{
    public string ProfileName => nameTextBox.Text;
    public string SystemPrompt => systemPromptTextBox.Text;
    public double Temperature => (double)temperatureNumeric.Value;
    public int MaxOutputTokens => (int)maxOutputTokensNumeric.Value;
    public bool IsDeleted { get; private set; }

    private readonly bool _isEditMode;

    public ProfileForm(string? name = null, string? systemPrompt = null, double temperature = 0, int maxOutputTokens = 8192)
    {
        InitializeComponent();

        _isEditMode = name is not null;

        if (_isEditMode)
        {
            Text = "Edit Profile";
            nameTextBox.Text = name;
            systemPromptTextBox.Text = systemPrompt ?? string.Empty;
            temperatureNumeric.Value = (decimal)Math.Clamp(temperature, 0, 1);
            maxOutputTokensNumeric.Value = Math.Clamp(maxOutputTokens, 1, 128000);
            deleteButton.Visible = true;
        }
        else
        {
            Text = "New Profile";
            temperatureNumeric.Value = (decimal)temperature;
            maxOutputTokensNumeric.Value = maxOutputTokens;
            deleteButton.Visible = false;
        }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            MessageBox.Show("Profile name cannot be empty.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            $"Are you sure you want to delete the profile \"{nameTextBox.Text}\"?",
            "Delete Profile",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            IsDeleted = true;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
