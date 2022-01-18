FROM node:lts-alpine AS base
ARG ArrivalsApi_Url
ENV ArrivalsApi_Url=$ArrivalsApi_Url
RUN npm install -g http-server
WORKDIR /app
EXPOSE 80

FROM node:lts-alpine AS build
WORKDIR /src
COPY Web/CovidStat.Arrivals.Client/package*.json ./
RUN npm ci
COPY Web/CovidStat.Arrivals.Client/ ./
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=build /src/dist ./dist
CMD [ "sh", "-c", "http-server dist --port 80 --proxy ${ArrivalsApi_Url}"]
