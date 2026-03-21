using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cellm.AddIn.UserInterface.Ribbon;

public partial class RibbonMain
{
    private enum ActionsGroupControlIds
    {
        ActionsGroup,
        ConvertSelectionButton,
        ConvertSheetButton,
        ConvertWorkbookButton,
    }

    public string ActionsGroup()
    {
        return $"""
        <group id="{nameof(ActionsGroupControlIds.ActionsGroup)}" label="Actions">
            <button id="{nameof(ActionsGroupControlIds.ConvertSelectionButton)}"
                    size="normal"
                    label="Selection"
                    imageMso="PasteSpecial"
                    onAction="{nameof(OnConvertSelectionToValues)}"
                    screentip="Convert to values"
                    supertip="Replace PROMPT formulas in the selected cells with their computed values." />
            <button id="{nameof(ActionsGroupControlIds.ConvertSheetButton)}"
                    size="normal"
                    label="Sheet"
                    imageMso="PasteValues"
                    onAction="{nameof(OnConvertSheetToValues)}"
                    screentip="Convert to values"
                    supertip="Replace all PROMPT formulas in the current sheet with their computed values." />
            <button id="{nameof(ActionsGroupControlIds.ConvertWorkbookButton)}"
                    size="normal"
                    label="Workbook"
                    imageMso="PasteAllMergeConditionalFormats"
                    onAction="{nameof(OnConvertWorkbookToValues)}"
                    screentip="Convert to values"
                    supertip="Replace all PROMPT formulas in all sheets with their computed values." />
        </group>
        """;
    }

    public void OnConvertSelectionToValues(IRibbonControl control)
    {
        ConvertRangeToValues(Application.Selection as Excel.Range);
    }

    public void OnConvertSheetToValues(IRibbonControl control)
    {
        if (Application.ActiveSheet is Excel.Worksheet sheet)
        {
            ConvertRangeToValues(sheet.UsedRange);
        }
    }

    public void OnConvertWorkbookToValues(IRibbonControl control)
    {
        if (Application.ActiveWorkbook is Excel.Workbook workbook)
        {
            foreach (Excel.Worksheet sheet in workbook.Worksheets)
            {
                ConvertRangeToValues(sheet.UsedRange);
            }
        }
    }

    private static void ConvertRangeToValues(Excel.Range? range)
    {
        if (range == null)
        {
            return;
        }

        ExcelAsyncUtil.QueueAsMacro(() =>
        {
            foreach (Excel.Range cell in range.Cells)
            {
                try
                {
                    if (cell.HasFormula is true)
                    {
                        var formula = (string)cell.Formula;
                        if (formula.Contains("PROMPT", StringComparison.OrdinalIgnoreCase))
                        {
                            cell.Value = cell.Value;
                        }
                    }
                }
                catch (Exception)
                {
                    // Skip cells that cause errors (e.g., merged cells, protected cells)
                }
            }
        });
    }
}
