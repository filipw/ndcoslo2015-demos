# NDC Oslo, June 2015

Demo of migrating Web API to MVC6 from NDC Oslo 2015

## Video

[Watch the talk on Vimeo](https://vimeo.com/131633175)

## Slides

Clone the repo, navigate [here](https://github.com/filipw/ndcoslo2015-demos/tree/master/slides/FsReveal) and run `build.cmd` (win) or `build.sh` (*nix).
Alternatively, just go [to this URL](http://filipw.github.io/ndcoslo2015).

Mind you this talk is all about code so there aren't many slides anyway.

## Code

This repo contains:

 - the [Web API 2 Contacts Manager](https://github.com/filipw/ndcoslo2015-demos/tree/master/web%20api%20contacts%20manager/ContactsManager)
 - the [MVC 6 Contacts Manager, empty project](https://github.com/filipw/ndcoslo2015-demos/tree/master/mvc6%20contacts%20manager%20before/ContactManager)
 - the [MVC 6 Contacts Manager, final project](https://github.com/filipw/ndcoslo2015-demos/tree/master/mvc6%20contacts%20manager%20after/ContactManager)
 
The MVC 6 projects were built using the latest (at the time) nightlies - `1.0.0-beta6-12189`.
To run the project make sure you install that DNX version:

 - `dnvm install 1.0.0-beta6-12189 -u`
 - `dnvm install 1.0.0-beta6-12189 -u -runtime coreclr`

Alternatively go to `global.json` in the MVC 6 projects and update the DNX version there

Thanks!
