<template>
  <el-form ref="ruleForm" :model="form" :rules="rules" label-width="100px">
    <el-form-item label="产品名称" prop="proName">
      <el-input v-model="form.proName" placeholder="请输入产品名称" style="width:80%;"></el-input>
    </el-form-item>
    <!-- <el-form-item label="产品编码">
      <el-input v-model="form.proCode" placeholder="请输入产品编码" style="width:80%;"></el-input>
    </el-form-item>-->
    <el-form-item label="产品分类" prop="categoryId">
      <el-cascader
        placeholder="请选择产品分类"
        v-model="form.categoryId"
        :options="procateoptions"
        :props="{ checkStrictly: true,emitPath:false }"
        clearable
        style="width:80%;"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="产品基本单位">
      <el-input v-model="form.proBaseUnit" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品图片" prop="fileId">
      <el-upload
        :action="uploadAddr"
        list-type="picture-card"
        :auto-upload="true"
        :limit="1"
        :v-model="form.fileId"
        :on-success="uploadSuccess"
        :on-exceed="uploadExceed"
        :on-preview="handlePictureCardPreview"
        :on-remove="handleRemove"
      >
        <i class="el-icon-plus"></i>
      </el-upload>
      <el-dialog :visible.sync="dialogVisible" :modal="false">
        <img width="100%" :src="dialogImageUrl" alt />
      </el-dialog>
    </el-form-item>
    <el-form-item label="产品描述">
      <el-input type="textarea" v-model="form.proDesc"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('ruleForm')">立即创建</el-button>
      <el-button  @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>
<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      uploadAddr: http.rootApi + "/upload/UploadImage",
      fit: "contain",
      url: "",
      dialogImageUrl: "",
      dialogVisible: false,
      disabled: false,
      uploadVisible: true,
      procateoptions: [],
      form: {
        proName: "",
        //proCode: "",
        proBaseUnit: "",
        categoryId: "",
        fileId: "",
        proDesc: ""
      },
      rules: {
        proName: [
          { required: true, message: "请输入活动产品名称", trigger: "blur" }
        ],
        categoryId: [
          {
            required: true,
            message: "请选择产品分类",
            trigger: "change"
          }
        ],
        fileId: [
          {
            required: true,
            message: "请上传图片",
            trigger: "change"
          }
        ]
      }
    };
  },
  mounted() {
    this.getCategoryList();
  },
  methods: {
    uploadSuccess(data) {
      if (data.data && data.data.id) {
        this.form.fileId = data.data.id;
        this.$refs["ruleForm"].validate();
      }
    },
    uploadExceed() {
      this.$message("只能上传一张图片");
    },
    getCategoryList() {
      var api = "/ProCategory/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.procateoptions = response.data;
        } else {
          this.procateoptions.clear();
        }
      });
    },
    handleRemove() {
      this.form.fileId = "";
      this.$refs["ruleForm"].validate();
    },
    handlePictureCardPreview(file) {
      this.dialogImageUrl = file.url;
      this.dialogVisible = true;
    },
    handleDownload(file) {
      console.log(file);
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        //验证通过
        if (valid) {
          var api = "/ProInfo/Add";
          http.post(api, this.form, response => {
            if (response.success) {
              this.$message({
                message: "产品分类添加成功",
                type: "success"
              });
              //获取父级窗口是否正确
              if (this.$parent.$parent) {
                //关闭窗口
                this.$parent.$parent.dialogAddProduct = false;
                this.$parent.$parent.getData();
              }
            } else {
              this.$message.error(response.errMsg);
            }
          });
        } else {
          return false;
        }
      });
    },
    onCancel() {
      if (this.$parent.$parent) {
        this.$parent.$parent.dialogAddProduct = false;
      }
    }
  }
};
</script>