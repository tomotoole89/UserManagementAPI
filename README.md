Below is how copilot helped create this api.

It create the base structure with each of the crud endpoints. also created the repository and model files. helped get swagger workng when installed wrong package, as couldnt use postman.

gave options to validate input ended up using FluentValidation. optimized repository by change from list to dictionary and other placing to change for it to work.

added the middleware for error handling and logging.
debug issue with swagger and authorization. had to drowngrade to .net8 and change versions of packages for it to work. generating the middleware code to add tokens