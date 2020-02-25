<template>
  <el-form ref="form" :model="form" label-width="80px">
    <el-form-item label="父级分类">
      <el-select v-model="form.procatevalue" placeholder="请选择">
        <el-option
          v-for="item in form.procateoptions"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="分类名称">
      <el-input v-model="form.procatename"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit" >添加</el-button>     
      <el-button>取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  data() {
    return {
      form: {
        procateoptions: [
        ],
        procatename: ""
      }
    };
  },
  mounted:function(){

  },
  methods:{
      initData(){
          var api = 'http://127.0.0.1:8888/api/ProCategory/GetDropDownListAsync';
          
      },
      onSubmit(){
        
      }
  }
};
</script>