package SparxSystems;

import org.sparx.*;

/*
 * @author sparxsys
 * @version 1.0
 * @created 29-Jun-2016 2:59:37 PM
 * @type Application Pattern
 * @description 
 *  This program template was created using the Application Patterns available in Enterprise Architect
 * @environment
 *  Enterprise Architect automation interface 
 *  Java
 * @goal
 *  To extend the boundaries of the traditional and occasionally limited scripting environment 
 *  to empower users working with Enterterprise Architect models to take advantage of the more 
 *  powerfull features of high level languages such as Java.
 */
public class RepositoryInterface {

	private int m_eapid = 0;
	private org.sparx.Repository m_repository = null;
	
	public RepositoryInterface(){

	}
	public RepositoryInterface(int processID){
		// The processID above is the operating system process ID of the model provider
		// It is passed automatically when an Application Pattern is ran from Enterprise Architect
		m_eapid = processID;
		// Obtain the Repository class for the model provided by the Enterprise Architect process
		m_repository = Services.GetRepository(processID);
	}
	
	public void Trace( String msg )
	{
		// You can change the System Output Tab that receives the trace messages. This demo uses the 'Script' tab
		m_repository.WriteOutput( "Script", msg, 0);
		System.out.println( msg);
	}

	public void PrintPackage( org.sparx.Package pkg)
	{
		Trace( pkg.GetName());
		Collection<org.sparx.Package> packages = pkg.GetPackages();
		for(short i = 0; i < packages.GetCount(); i++)
		{
			PrintPackage(packages.GetAt(i));
		}
	}
	
	public void Demo()
	{
		// In this demo we will simply print the names of every package in the model
		Collection<org.sparx.Package> packages = m_repository.GetModels();
		for(short i = 0; i < packages.GetCount(); i++)
		{
			org.sparx.Package pkg = packages.GetAt(i);
			PrintPackage(pkg);
		}
	}

	// cleanup
	public void finalize() throws Throwable {
	}
	
	// the main entry point
	public static void main( String[] args)
	{
		try
		{
			int pid = 0;
			if(args.length>0)
			{
				pid = Integer.parseInt(args[0]);
			}
			if(pid>0)
			{
				RepositoryInterface app = new RepositoryInterface(pid);
				app.Demo();
			}
		}
		catch( Exception e)
		{
			System.out.println("exception: " + e.toString());
		}
		System.out.println("Finished");
	}
	
}//end RepositoryInterface