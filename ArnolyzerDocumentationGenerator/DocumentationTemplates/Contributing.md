# Contributing to Arnolyzer Analyzers #

This page is still work in progress. For now, I've simply documented how to get the thing building, as the process is still a little rough around the edges, to say the least. More information on how to make changes will appear soon (I hope! Such promises are all too often "famous last words" :smiling:)

## Forking, cloning and building ##
### Forking ###
To start off, create a fork of the [Arnolyzer project](https://github.com/DavidArno/Arnolyzer) to your own account, then clone it to your local machine. That's the simple part!

### Creating the wiki and `gh-pages` directories ###

Next, as the documents in both the wiki and website are created by building the Arnolyzer solution, you need to:
1. Create an empty directory for the wiki pages to be written to.
2. Clone the repository to your local machine a second time and switch that to the `gh-pages` branch.

By way of example, I'll assume you have cloned the repository to `C:\Development\Arnolyzer`. Having done that:
1. Create a directory `C:\Development\Arnolyzer.wiki`.
2. Create a directory `C:\Development\Arnolyzer_pages`.
3. In the latter directory, run `git clone https://github.com/<your user name>/Arnolyzer.git`, then `git branch --set-upstream gh-pages origin/gh-pages`.

You are now all set up to start building the solution. When you do so, the wiki pages will be created in the `C:\Development\Arnolyzer.wiki` directory and the website will be created in `C:\Development\Arnolyzer_pages`.

If you want to use something other than `C:\Development` as the base directory, you can. The important thing is that `Arnolyzer`, `Arnolyzer.wiki` and `Arnolyzer_pages` must all be in the same directory.
