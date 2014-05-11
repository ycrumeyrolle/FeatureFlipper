#FeatureFlipper

*A feature flipping system.*

Feature flipping enable you to integrate your code earlier, avoiding feature branching.
It can also be used to provide specific behaviour to specific users (A/B testing, beta-test users).
http://martinfowler.com/bliki/FeatureToggle.html

Example of use:


```C#


    if(Features.Flipper.IsOn("feature1")) 
    {
        // do the job
    }


```


#How to use:

###1. Download NuGet package
https://www.nuget.org/packages/FeatureFlipper


###2. Define some features

In your configuration file : 

```XML

	...
	<appSettings>
		<add key="FeatureFlipper.Sample.FeatureByType" value="true" /> 
		<add key="featureByKey" value="true" />
		<add key="featureByDate" value="2000-01-01T00:00:00+00:00" />
	</appSettings>
	...
	
```

###3. Add code that is conditional on feature availability
	
```C#

    // By type feature flipping
    if (Features.Flipper.IsOn<FeatureByType>())
    {
        // do feature enabled by type ...
    }

    // By key feature flipping
    if (Features.Flipper.IsOn("featureByKey"))
    {
        // do feature enabled by key ...
    }

    // By date feature flipping
    if (Features.Flipper.IsOn("featureByDate"))
    {
        // do feature enabled by date
    }

	
```

#Go further

FeatureFlipper offer many extensibility points:
 
##Create your custom ```IFeatureProvider```

A ```IFeatureProvider``` simply said "Hey ! I know this feature and it is ON (or OFF) !".

By default, FeatureFlipper provides ```ConfigurationFeatureProvider``` and ```PerRoleFeatureProvider```.

See [Extending the features provider](https://github.com/ycrumeyrolle/FeatureFlipper/wiki/Extending-the-features-provider) for more details.


##Create your custom ```IConfigurationReader```

The ```IConfigurationReader``` interface is used by the ```ConfigurationFeatureProvider```. It provides the value of a feature given a feature key. 

The default implementation reads the value into the app/web.config file, in the appSettings section.
You could create your own implementation by reading values from a SQL/NoSQL database, a web service or anything else 

See [Extending the configuration reading](https://github.com/ycrumeyrolle/FeatureFlipper/wiki/Extending-the-configuration-reading) for more details.


##Create your custom ```IFeatureStateParser```
The ```IFeatureStateParser``` interface is used by the ```ConfigurationFeatureProvider```. It provides a boolean value for a feature given a feature state. 
The defaults implementations converts boolean strings to boolean values ("true" ==> ON; "false" ==> OFF). 
It can also convert dates representing string to boolean values, based on the predicate that past date means feature ON, an future date means feature OFF. 

See [Extending the configuration parsing](https://github.com/ycrumeyrolle/FeatureFlipper/wiki/Extending-the-configuration-parsing) for more details.


##Ioc via [Unity](http://unity.codeplex.com/)
The NuGet package [FeatureFlipper.Unity](https://www.nuget.org/packages/FeatureFlipper) wire FeatureFlipper with Unity. 
Feature flipping became much more easier with IoC : 

In your configuration file : 


```XML

	...
	<appSettings>
		<add key="FeatureFlipper.Unity.Sample.EmailSender" value="true" /> 
	</appSettings>
	...
	
```

In your Ioc configuration :

```C#

    // Add the Feature flipping extensions
    container.AddFeatureFlippingExtension();
	
```

In your code :

```C#
    
    // Get an object from the IoC container. 
    // This could be done anywhere in your code, for example by service location, constructor injection or property injection.
    IMessageSender sender = container.Resolve<IMessageSender>();
    
    // Call the code. Does not worry with 'if' statement.
    sender.SendMessage("This message is sent only if the feature is enabled.");
	
```

It is unnecessary to add a 'if' statement. 
When the feature is enabled, the Ioc container returns the desired object an its method is called.
When the feature is disabled, the Ioc container returns a [NullObject](http://refactoring.com/catalog/introduceNullObject.html). Any method called on this object does nothing.
It is a bit similar to the [ConditionalAttribute](http://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute.aspx).

#License & Copyright


This software is released under the MIT Licence. It is Copyright 2014, Yann Crumeyrolle. I may be contacted at ycrumeyrolle@free.fr.

