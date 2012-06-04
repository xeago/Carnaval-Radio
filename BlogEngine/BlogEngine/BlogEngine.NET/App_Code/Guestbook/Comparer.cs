using System.Collections;

namespace Guestbook
{
    public class Comparer : IComparer
    {
        #region IComparer Members

        public int Compare(object poObject1, object poObject2)
        {
            var liValue1 = (int) poObject1;
            var liValue2 = (int) poObject2;

            if ((liValue1 < liValue2))
            {
                return -1;
            }

            if ((liValue1 > liValue2))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}