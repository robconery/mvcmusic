ASP.NET MVC Music Store, Updated
===================================
This repo started life as a way to do a Tekpub episode and contribute back to an Open Source project in the meantime. 
What you see in this repo is our effort to "clean up" a few things, pull this project into the modern day, and add some 
light functionality.

This is described below.

## Twitter Bootstrap
The CSS for the project was fine, but there are things every web site needs and Twitter Bootstrap can really help with that.
We've added that to every page, while trying to keep the look and feel the same.

## Industry Standard Structure
The original project works fine, but doesn't really follow the Controller/Action naming standards used by Rails and followed
by many other frameworks - including ASP.NET MVC itself! So we cleaned up the controllers to reflect the common ideas used
in the industry.

## Better Use Of EntityFramework
One mistake that people getting used to EntityFramework will make is "flushing" the data context (aka calling "SaveChanges") whenever they
want to see a model change. This is a **very bad idea** because the DataContext is a UnitOfWork - meaning that other models at other
points in the application might have queued changes, waiting for other models to change as well.

The only point you should SaveChanges() is the last possible moment. Which is usually at the very end of the request.

For that reason we've included a Controller Base Class called "DbController". You could also use Inversion of Control for this, but (as pointed out below)
this demo app doesn't use it, and we want to comply with the ASP.NET Team's wishes.

Have a look at how you can open a session, make it available to your models, and then save the changes later on - disposing when all is said and done.

## KnockoutJS
The whole point of this exercise is to show off KnockoutJS. Over the coming week we'll be augmenting some of the site functionality
to use Knockout effectively. What does "effectively" mean? It means "not just because" - we want to use it where it will **greatly improve the user experience**.

You can follow what we do in the Order/Edit bits.

## Helpers
We've added a few helpers to work with Bootstrap as well as get around some of the amazing craziness in the HTML Helpers. You can find
our additions in the App_Code directory.

## Code Templates
We've rewritten the core code templates to use Bootstrap, adding some nice eye-candy and confirmations on delete.

## Help? 
If you want to help - feel free! We won't be accepting **any Resharper Forks** - so please don't request a pull if you've "run Resharper for us".
However, we'd love any contribution that fixes a bug or enhances the project overall.

This site is meant to be a super-beginner demonstration of process and concept. There's no IoC and the overall "concept count" is kept to a minimum.
This isn't our choice - but we're honoring what the ASP.NET Team wants to do here.

Hopefully some of the ideas you see here might work their way back to the origin demo.
