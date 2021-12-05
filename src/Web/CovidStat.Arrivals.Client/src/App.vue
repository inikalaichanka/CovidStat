<template>
  <div id="app">
    <Arrivals/>
  </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import { BootstrapVue } from 'bootstrap-vue'

    import 'bootstrap/dist/css/bootstrap.css'
    import 'bootstrap-vue/dist/bootstrap-vue.css'

    import Arrivals from './components/Arrivals.vue'


    Vue.use(BootstrapVue)
    export default Vue.extend({
        name: 'App',
        components: {
            Arrivals
        }
    });
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin-top: 60px;
}
</style>
