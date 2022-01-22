import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Vue from 'vue';

export default {
    async install () {
        const arrivalsHub = new Vue();
        Vue.prototype.$arrivalsHub = arrivalsHub;

        const connection = new HubConnectionBuilder()
            .withUrl('/arrivals-hub')
            .configureLogging(LogLevel.Information)
            .build();

        connection.on('ReceiveArrival', (arrival) => {
            arrivalsHub.$emit('arrival-received', arrival);
        });

        await connection.start();
    }
};