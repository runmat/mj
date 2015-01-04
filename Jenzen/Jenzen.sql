declare @rubrik varchar(5)

set @rubrik = 'k'


select 
datepart(yy,Datum) Jahr,
--datepart(m,Datum) Monat,
--Rubrik,
sum(Betrag) / 12 Betrag
from Buchungen

where datepart(yy,Datum) = 2014
--and Rubrik = @rubrik
and Rubrik is not null and Rubrik <> 'bud'

group by 
--Rubrik,
datepart(yy,Datum) 
--, datepart(m,Datum) 



select 
datepart(yy,Datum) Jahr,
--datepart(m,Datum) Monat,
Rubrik,
sum(Betrag) / 12 Betrag
from Buchungen

where datepart(yy,Datum) = 2014
--and Rubrik = @rubrik
and Rubrik is not null and Rubrik <> 'bud'

group by Rubrik,
datepart(yy,Datum) 
--, datepart(m,Datum) 

order by 
Rubrik,
datepart(yy,Datum)
--, datepart(m,Datum) 


select 
datepart(yy,Datum) Jahr,
datepart(m,Datum) Monat,
Rubrik,
sum(Betrag) Betrag
from Buchungen

where datepart(yy,Datum) = 2014
--and Rubrik = @rubrik
and Rubrik is not null and Rubrik <> 'bud'

group by Rubrik,
datepart(yy,Datum) 
, datepart(m,Datum) 

order by
Rubrik, 
datepart(yy,Datum)
, datepart(m,Datum) 
