namespace AutoAct.Enums
{
    public enum VehicleLifetime
    {
        NEW,	                // Neu                  -> Zulassungsdatum muss nicht angegeben sein
        USED,	                // Gebraucht	        -> Zulassungsdatum muss angegeben sein und darf nicht in der Zukunft sein
        ONE_DAY_REGISTRATION,	// 1-Tag-Zulassung	    -> Zulassungsdatum muss angegeben sein und darf nicht in der Zukunft sein
        ONE_YEAR_REGISTRATION,	// 1-Jahr-Zulassung	    -> Zulassungsdatum muss angegeben sein und darf nicht mehr als 15 Monaten in der Vergangenheit liegen
        DEMONSTRATION,	        // Vorführfahrzeug      -> Zulassungsdatum muss angegeben sein und darf nicht in der Zukunft sein
        OLDTIMER	            // Oldtimer	            -> Zulassungsdatum muss mindestens 20 Jahren in der Vergangenheit liegen
    }
}
