# Create a database

<code> 

create table typeDocument (
	id int identity primary key,
	nameType varchar(max)
)

create table tag (
	id int identity primary key,
	tag varchar(max)
)

create table typeDocumentTag (
	id int identity primary key,
	typeDocumentId int,
	tagId int,
	foreign key(typeDocumentId) references typeDocument(id),
	foreign key(tagId) references tag(id)
)

create table document (
	id int identity primary key,
	[name] varchar(max),
	fileXml xml,
	typeDocumentId int,
	foreign key(typeDocumentId) references typeDocument(id)
)

create table documentTag (
	id int identity primary key,
	documentId int,
	tagId int,
	[value] varchar(max),
	foreign key(documentId) references document(id),
	foreign key(tagId) references tag(id)
)

insert into typeDocument (nameType) 
	values 
	('protocol'),
	('notification')

insert into tag (tag) 
	values 
	('memberNumber'), --1
	('lastName'), --2	
	('firstName'), --3
	('middleName'), --4
	('commissionRole'), --5
	('commissionName'), --6
	('fullName'), --7
	('factAdress'), --8
	('INN'), --9
	('KPP'), --10
	('regNum'), --11
	('purchaseNumber'), --12
 	('docNumber'), --13
	('docPublishDTInEIS'), --14
	('href'), --15
	('fileName'), --16
 	('url'), --17
	('appNumber'), --18
	('appDT'), --19
 	('finalPrice'), --20
	('OKPDCode'), --21
	('OKPDName'),  --22
	('name')  --23

insert into typeDocumentTag (typeDocumentId, tagId) 
	values 
	(1, 1),
	(1, 2),
	(1, 3),
	(1, 4),
	(1, 5),
	(1, 6),
	(1, 7),
	(1, 8),
	(1, 9),
	(1, 10),
	(1, 11),
	(1, 12),
	(1, 13),
	(1, 14),
	(1, 15),
	(1, 18),
	(1, 19),
	(1, 20),
	(2, 7),
	(2, 8),
	(2, 9),
	(2, 10),
	(2, 11),
	(2, 12),
	(2, 13),
	(2, 14),
	(2, 15),
	(2, 16),
	(2, 17),
	(2, 21),
	(2, 22),
	(2, 23)	

# Script for DB

DECLARE @fileX xml;
SELECT @fileX  = (SELECT * FROM OPENROWSET (BULK 'C:\Users\nuran\OneDrive\Рабочий стол\david\ProjectC\CouksMakovii\epProtocolEF2020SubmitOffers_0307300044422000172_40052725.xml', SINGLE_BLOB) as [xml])
insert into document (name, fileXml, typeDocumentId) 
 values 
 (@name, @fileX, @typeDocumentId)

insert into documentTag (documentId, tagId, [value]) 
	values 
	(@documentId, @tagId, @[value])

select document.id as 'Номер документа',  typeDocument.nameType as 'Тип', document.name as 'Название документа',
		tag.tag as 'Тэг', documentTag.value as 'Значение', document.fileXml as 'xml'
from documentTag, document, tag, typeDocument, typeDocumentTag
where 
	  documentTag.documentId = document.id and
	  documentTag.tagId = tag.id  and 
	  typeDocument.id = document.typeDocumentId and
	  typeDocument.id = typeDocumentTag.typeDocumentId and
	  tag.id = typeDocumentTag.tagId

</code>
   
<hr/>