<template>
  <div id="app">
    <MenuList />
    <AddCategory />
  </div>
</template>

<script>
// import HelloWorld from './components/HelloWorld.vue'
// import AddPro from "./components/product/AddPro.vue";
// import AddCategory from "./components/procategory/AddCategory.vue";
import AddCategory from "./components/procategory/AddCategory.vue";
import MenuList from "./components/menu/munulist.vue";

export default {
  name: "App",
  components: {
    // HelloWorld,
    MenuList,
    AddCategory
  }
};
</script>

<style>
</style>
