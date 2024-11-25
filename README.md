# git-auto-commit
Have you ever wondered what was the flow of transformations on a code but git diff does not show you a correct picture.

Tadaaa - here I present a solution, at least I think it is a solution:

create a commit on each change of the file.



## supported scenario:
1) you have one "main" branch from which you create feature branches
2) all your projects are located in one folder (works for me at least :D )

I created this project within three days and I think that I am completely satisfied with it [as of 2024-11-26]

## remarks aka known issues

[ ] also push every commit - nah, I mean no need to do this. Pushing commits is not that hard and this allows for better privacy support

[ ] there are a lot of branches, I need to remove automatically them

[ ] get default branch name (not everyone uses Main or prod)

[ ] run own instance of git-server with withible commit details
# Support on Patreon

https://www.patreon.com/o_kryvonos


# Join Discord server 

https://discord.gg/W5HFSKVXgc

## Building

```
dotnet run
```

## License

Copyright © Kryvonos Oleksandr 2024 <oleksandr.kryvonos.ods.ua@gmail.com>. Provided under the [MIT License](http://opensource.org/licenses/MIT).
