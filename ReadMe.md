# Pokemon

This provides an API to query for Pokemon information.

## Overview

There are two end points:

1. /pokemon/{pokemonName}
2. /pokemon/translated/{pokemonName}

## Instructions on how to run

After cloning the project , step inside the root folder where the .sln file is in and in the terminal/command prompt type `docker-compose up --build --abort-on-container-exit`

The application starts up on port 9001.

Use this link in your browser to navigate to the swagger page.

`http://localhost:9001/swagger/index.html`

## Implementation details

I have tried to keep it simple. When a request comes to either of the end points it is then handed off to the PokemonSerivce class to deal with it.

I have added minimal logging mainly for error conditions.

There is also an integration test project which tests all the happy case scenarios.

## Further enhancements needed for Production env

- A heart beat end point to know if the service is up.

- Adding scripts to run the tests and adding more tests for edge cases.

- The various end points to get the basic information and get translation data are at the moment hardcoded, I would have like to move this information to the configuartion file.

- The transaltion end points in the free version are rate limited, I have not done any rate limiting at application to stop that from exceeding the rate limit. One way to do this in production would be monitor the number of request coming in and putting rate limiting at the load balancer level.

- I would have liked to create a CI pipeline which shows the lates build of this project.

- If the service is to be scaled for millions of users then some kind of caching strategy can be thought of which caches all the pokemon information and retreives basic information from the cache. In this scenario we can delegate fetching of basic information to another service.
