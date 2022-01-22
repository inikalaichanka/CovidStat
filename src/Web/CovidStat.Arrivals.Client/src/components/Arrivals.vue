<template>
    <div class="arrivals">
        <div v-if="loading" class="text-center">
            <b-spinner variant="primary"></b-spinner>
        </div>

        <b-container class="text-center">
            <b-row v-for="arrival in arrivals" :key="arrival.id">
                <b-col md="8" offset-md="2">
                    <b-alert :variant="arrival.isVaccinated ? 'success' : 'danger'" show>
                        <p class="fw-light fst-italic text-end">{{ new Date(arrival.arrivalDate).toLocaleString() }}</p>
                        <p class="fs-4">{{ arrival.fullName }}</p>
                        <hr />
                        <p class="fs-5">
                            {{ arrival.region }}, {{ arrival.city }}<br />
                            {{ arrival.postalCode }}, {{ arrival.address }}
                        </p>
                        <hr />
                        <p class="fs-6">
                            {{ arrival.email }}<br />
                            {{ arrival.phone }}
                        </p>
                        <hr />
                        <p class="text-secondary">Departure at {{ new Date(arrival.departureDate).toLocaleString() }}</p>
                    </b-alert>
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    type Arrival = {
        id: string,
        fullName: string,
        phone: string,
        email: string,
        dateOfBirth: Date,
        region: string,
        city: string,
        address: string,
        postalCode: string,
        isVaccinated: boolean,
        arrivalDate: Date,
        departureDate: Date | null
    };

    interface Data {
        loading: boolean,
        arrivals: null | Arrival[]
    }

    export default Vue.extend({
        data(): Data {
            return {
                loading: false,
                arrivals: null
            };
        },
        created() {
            this.fetchData();
            this.$arrivalsHub.$on('arrival-received', this.onArrivalReceived);
        },
        watch: {
            '$route': 'fetchData'
        },
        methods: {
            async fetchData() {
                this.arrivals = null;
                this.loading = true;
                
                const count = 10;
                const data = await fetch(`/api/arrivals/latest?count=${count}`);

                this.arrivals = await data.json() as Arrival[];
                this.loading = false;
            },
            onArrivalReceived(arrival: Arrival) {
                this.arrivals?.pop();
                this.arrivals?.splice(0, 0, arrival);
            }
        },
    });
</script>