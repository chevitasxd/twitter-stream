FROM node:alpine AS base
WORKDIR /app

FROM node:alpine AS build
WORKDIR /src
COPY package.json .
RUN npm install
COPY . .
RUN  npm run build-lint

FROM base AS final
WORKDIR /app
COPY --from=build /src .
ENTRYPOINT ["npm", "start"]