Insert Into News_Portal.dbo.Accounts VALUES('814334d0-c28e-43e2-83b1-404ffb01d03f','unknown','asd_');

INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'EXtronauts released',GETDATE(),'New boardgame designed by NASA engineers has been productized.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'SaltCity', GETDATE(),'New boardgame SaltCity is marching toward its kikstarter campaing','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES('70555247-1d57-4498-9f93-2ca3661a5f44','Aye my dark overlord', GETDATE(),'New chapter has been released to the well known game.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'solo', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'waaaaaaaaa',GETDATE(),'New boardgame designed by NASA engineers has been productized.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'vittu', GETDATE(),'New boardgame SaltCity is marching toward its kikstarter campaing','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'it might work some day', GETDATE(),'New chapter has been released to the well known game.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'accidents happen', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'dixit', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'arkham',GETDATE(),'New boardgame designed by NASA engineers has been productized.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'doom', GETDATE(),'New boardgame SaltCity is marching toward its kikstarter campaing','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'ticket to ride', GETDATE(),'New chapter has been released to the well known game.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'epic spell wars', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'colt express', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'dominion',GETDATE(),'New boardgame designed by NASA engineers has been productized.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'carcassone', GETDATE(),'New boardgame SaltCity is marching toward its kikstarter campaing','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'citadella', GETDATE(),'New chapter has been released to the well known game.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');
INSERT INTO News_Portal.dbo.Articles VALUES(NEWID(),'roadkill rally', GETDATE(),'No more solo/uno/etc. are alowed in sokovia.','blahblahblahblahblahblahblahblahblahblahblah',1,'814334d0-c28e-43e2-83b1-404ffb01d03f');

INSERT INTO News_Portal.dbo.Images(Id,News_Id,Article_Id,Image1)
SELECT 'f2172f01-0dc4-427f-91aa-74e66b06b4b4','70555247-1d57-4498-9f93-2ca3661a5f44','70555247-1d57-4498-9f93-2ca3661a5f44',BulkColumn 
FROM Openrowset( Bulk 'D:\shared\waf\WAF_Bead_bg5q8g\WAF_Bead_bg5q8g\Content\aye.jpg', Single_Blob) as Image
INSERT INTO News_Portal.dbo.Images(Id,News_Id,Article_Id,Image1)
SELECT 'f6172f01-0dc4-427f-91aa-74e66b06b4b4','70555247-1d57-4498-9f93-2ca3661a5f44','70555247-1d57-4498-9f93-2ca3661a5f44',BulkColumn 
FROM Openrowset( Bulk 'D:\shared\waf\WAF_Bead_bg5q8g\WAF_Bead_bg5q8g\Content\aye.jpg', Single_Blob) as Image