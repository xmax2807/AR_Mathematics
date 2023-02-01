using UnityEngine;
using UnityEngine.UI;

namespace Project.UI{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum LayoutType
        {
            Uniform,
            FitWidth,
            FitHeight,
            FixedRows,
            FixedColumns
        }
        
        [SerializeField] private Vector2 spacing;
        [SerializeField] private LayoutType layoutType = LayoutType.Uniform;
        
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] public bool AutoCellWidth = true;
        [SerializeField] public bool AutoCellHeight = true;
        
        public override void CalculateLayoutInputVertical()
        {
            if (layoutType != LayoutType.FixedColumns && layoutType != LayoutType.FixedRows)
            {
                AutoCellWidth = true;
                AutoCellHeight = true;
                
                var sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }
            
            if (layoutType == LayoutType.FitWidth || layoutType == LayoutType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            }
            else if (layoutType == LayoutType.FitHeight || layoutType == LayoutType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
            }

            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            //Declaring a variable here to avoid the property access
            var layoutPadding = padding;

            if (AutoCellWidth && columns != 0)
            {
                var cellWidth = (parentWidth / columns) - (spacing.x / columns)*(columns-1) - (layoutPadding.left / (float)columns) - (layoutPadding.right/(float)columns);
                cellSize.x = cellWidth;
            }

            if (AutoCellHeight && rows != 0)
            {
                var cellHeight = (parentHeight / rows) - (spacing.y / rows)*(rows-1) - (layoutPadding.top / (float)rows) - (layoutPadding.bottom/(float)rows);
                cellSize.y = cellHeight;
            }

            for (var i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + layoutPadding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + layoutPadding.top;
                
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void SetLayoutHorizontal()
        {
            //Layout is set in CalculateLayoutInputVertical
            CalculateLayoutInputHorizontal();
        }

        public override void SetLayoutVertical()
        {
            //Layout is set in CalculateLayoutInputVertical
            CalculateLayoutInputVertical();
        }
        public void SetMaxColumn(int maxCol){
            layoutType = LayoutType.FixedColumns;
            columns = maxCol;
        }
        public void SetMaxRow(int maxRow){
            layoutType = LayoutType.FixedRows;
            rows = maxRow;
        }
    }
}