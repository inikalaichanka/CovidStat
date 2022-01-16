FROM node:lts-alpine
ARG ArrivalsApi_Url
ENV ArrivalsApi_Url=$ArrivalsApi_Url
RUN npm install -g http-server
WORKDIR /app
COPY Web/CovidStat.Arrivals.Client/package*.json ./
RUN npm install
COPY Web/CovidStat.Arrivals.Client/ ./
RUN npm run build
EXPOSE 80
CMD [ "sh", "-c", "http-server dist --port 80 --proxy ${ArrivalsApi_Url}"]
