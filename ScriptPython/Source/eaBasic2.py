def getSelectedObject(rep):
    """
    Get the selected object. It returns a tuple:
    - type, object
    """
    type = rep.GetContextItemType()
    obj = None
    # Check if type is plausible
    if (type > 0): 
         obj = rep.GetContextObject()
         name = obj.Name  
         print("Type of selected EA object is: {0}, , Name={1}".format(eacodes.EACodes.getTypeName(type), name))
    else:
         print("No type of selected EA object available, Type={0}".format(type))
        
        
    return (type, obj)

def trace(rep, msg):
    tabName = "TestEA" 
    rep.CreateOutputTab (tabName)
    rep.EnsureOutputVisible(tabName)
    rep.WriteOutput(tabName,msg,0)
    
    
import eacodes # Definition of EA codes
import argparse
import win32com.client



repositoryPath = "c:\Temp\Software_Architekturdesign_WLE.eap"
#repositoryPath = "c:/Temp/ScriptDotNet.eap"
repositoryPath1 = "c:/Temp/ScriptDotNet.eap"

# get the current EA object, from the running instance
eaApp = win32com.client.Dispatch("EA.App")
rep = eaApp.Repository
# check if connection was successful, the connection uses the first open EA instance
if (rep is None):
      print("No Repository found in first EA instance available", )
else:
    print("Repository '{repository:s}' found in first running instance".format(repository=rep.ConnectionString))
    type, obj = getSelectedObject(rep)
    trace(rep, "myTrace")
    rep = None
    eaApp = None

repName = repositoryPath1
print("\n\n=====================================\nTry to open a repository", repName)
eaApp = win32com.client.Dispatch("EA.App")  
eaApp.Repository.OpenFile(repName)
rep1 = eaApp.Repository
if (rep1 is None):
    print("Repository not found".format(repName))
else:
    print("Repository found", rep1.ConnectionString)
    trace(rep1, "\n========\nRepositoryOpened {0}".format(rep1.ConnectionString))
    type, obj = getSelectedObject(rep1)
    trace(rep1, "myTraceLoadedRepository '{0}".format(rep1.ConnectionString))
    
    
   
