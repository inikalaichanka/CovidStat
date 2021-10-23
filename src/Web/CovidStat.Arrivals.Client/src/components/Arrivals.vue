<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading...
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Full Name</th>
                        <th>Phone</th>
                        <th>E-mail</th>
                        <th>Date Of Birth</th>
                        <th>Region</th>
                        <th>City</th>
                        <th>Address</th>
                        <th>PostalCode</th>
                        <th>Vaccinated</th>
                        <th>Arrival Date</th>
                        <th>Departure Date</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="forecast in post" :key="forecast.date">
                        <td>{{ forecast.id }}</td>
                        <td>{{ forecast.fullName }}</td>
                        <td>{{ forecast.phone }}</td>
                        <td>{{ forecast.email }}</td>
                        <td>{{ forecast.dateOfBirth }}</td>
                        <td>{{ forecast.region }}</td>
                        <td>{{ forecast.city }}</td>
                        <td>{{ forecast.address }}</td>
                        <td>{{ forecast.postalCode }}</td>
                        <td>{{ forecast.isVaccinated }}</td>
                        <td>{{ forecast.arrivalDate }}</td>
                        <td>{{ forecast.departureDate }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    type Arrivals = {
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
    }[];

    interface Data {
        loading: boolean,
        post: null | Arrivals
    }

    export default Vue.extend({
        data(): Data {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.post = null;
                this.loading = true;

                fetch('arrivals')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as Arrivals;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>