using DevExpress.XtraPivotGrid.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanIlCNS
{
    public class MyPivotLocalizer : PivotGridLocalizer
    {
        public override string GetLocalizedString(PivotGridStringId id)
        {
            switch (id)
            {
                case PivotGridStringId.ColumnHeadersCustomization:
                    return "여기에 상위 조건을 놓으세요.";
                case PivotGridStringId.FilterHeadersCustomization:
                    return "여기에 필터링할 조건을 놓으세요.";
                case PivotGridStringId.GrandTotal:
                    return "집계";

            }
            return base.GetLocalizedString(id);
        }
    }
}
