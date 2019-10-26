FROM node:alpine AS base
WORKDIR /app

FROM node:alpine AS build
WORKDIR /src
COPY . .
RUN npm install

FROM base AS final
WORKDIR /app
COPY --from=build /src .
ENTRYPOINT ["npm", "start"]