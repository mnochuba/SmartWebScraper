FROM node:20-alpine

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build
RUN npm install -g serve

CMD ["serve", "-s", "dist/app/browser", "-l", "80"]
