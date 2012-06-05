using System;

namespace Guestbook
{
    public class Utility
    {
        public static bool IsNumeric(object poObject)
        {
            try
            {
                int.Parse(poObject.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static int RoundUp(decimal num)
        {
            int functionReturnValue = 0;
            if ((num - Convert.ToInt32(num) != 0))
            {
                functionReturnValue = Convert.ToInt16(num - (num%1)) + 1; // Convert.ToInt16(num) + 1;
            }
            else
            {
                functionReturnValue = Convert.ToInt16(num);
            }
            return functionReturnValue;
        }

        public static void GetStartAndEnd(ref int liStart, ref int liEnd, int liTotalCount, int liPage, bool lbOrder,
                                          int piMsgPerPage)
        {
            if (lbOrder)
            {
                // asc
                if (liTotalCount < piMsgPerPage)
                {
                    liStart = 0;
                    liEnd = liTotalCount;
                }
                else
                {
                    //liStart = (liPage - 1) * piMsgPerPage;
                    //liEnd = (liStart + piMsgPerPage) - 1;
                    liStart = (liPage - 1)*piMsgPerPage;
                    liEnd = (liStart + piMsgPerPage);
                    if (liEnd > liTotalCount)
                    {
                        liEnd = liTotalCount;
                    }
                }
            }
            else
            {
                // desc
                if (liTotalCount < piMsgPerPage)
                {
                    liStart = liTotalCount;
                    liEnd = 0;
                }
                else
                {
                    liStart = liTotalCount - ((liPage - 1)*piMsgPerPage);
                    liEnd = (liStart - piMsgPerPage); // +1;
                    if (liEnd < 0)
                    {
                        liEnd = 0;
                    }
                }
            }
        }

        public static string GenerateNavigation(int liTotalCount, int liPage, bool lbOrder, string psPageName,
                                                int piMsgPerPage)
        {
            string lsNavigation = "";
            decimal ldRatio = liTotalCount/Convert.ToDecimal(piMsgPerPage);
            int i = 0;
            string lsOrder = null;

            lsOrder = lbOrder ? "desc" : "asc";

            if (ldRatio < 1)
            {
                lsNavigation = "Page <a href=\"List.aspx\">1</a>";
            }
            else
            {
                lsNavigation = "Page ";
                for (i = 1; i <= RoundUp(ldRatio); i++)
                {
                    lsNavigation += "<a href=\"" + psPageName + ".aspx?page=" + i + "&order=" + lsOrder + "\">" + i +
                                    "</a> ";
                }
            }

            return lsNavigation;
        }
    }
}