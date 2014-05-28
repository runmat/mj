using System;
using System.Data;

namespace TimeRegistration
{
    class TimeDayPositions
    {
        DateTime m_dtFirstKommen;
        DateTime m_dtLastGehen;

        decimal m_Worktime;

        string strSatzartKommen;
        string strSatzartGehen;

        TimeDayPositions(DataRow[] rows)
        {
            //Übersetzungen holen
            strSatzartKommen = TimeRegistrator.TranslateAction(TimeRegistrator.TimeAction.Kommen);
            strSatzartGehen = TimeRegistrator.TranslateAction(TimeRegistrator.TimeAction.Gehen);

            // Grenzwerte berechnen
            getFirstKommen(ref rows);
            getLastGehen(ref rows);
        }

        private void getLastGehen(ref DataRow[] rows)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Liefert die erste Kommen-Zeit
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private DateTime getFirstKommen(ref DataRow[] rows)
        {
            DateTime dKommen = new DateTime();
            foreach (DataRow row in rows)
            {
                if (row["SATZART"].ToString() == strSatzartKommen)
                {
                    if (dKommen == null)
                    {

                    }
                }
            }
            dKommen = new DateTime();

            return dKommen;
        }
    }
}
