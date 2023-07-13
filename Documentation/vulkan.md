
Graphics.Device 
    Graphics.Device.Init( Functions, Handles, Config )
        CreateInstanceDebugAndSurface( Functions, Handles, Config.Device)
        
    Graphics.Device.Dispose( Functions, Handles )
        Physical device , logic device, optimal information
        => Device_OptimalValues and Device_Informations in data

Graphics.Render 
    Graphics.Render.Init( Functions, Handles, Config )
        SwapChain (imageView ), depth buffer (image view )
        Command Pool , Command buffers 
        Synchronization cache control
        RenderPass
        Framebuffer => need image view

    Graphics.Render.Draw(Functions, Handles, PipelineData )
        PREPARE A FRAME
        //SYNCHRONISATION OBJECTS :  USED TO CONTROL THE ORDER OF EXECUTION OF ASYNCHRONOUS OPERATION
        // ACQUIRE NEXT IMAGE 
        Update buffers
        RECORD COMMANDS WICH CAN THEN BE SUBMITED TO DEVICE QUEUE FOR EXECUTION
            // START THREAD => begin record command 
                Foreach Render Pass ( set render pass : framebuffers, color, surface, ...)
                //DRAW PIPELINE 
                    FOREACH SHADER =>    Set shdar state ( shader tesslation)
                        FOREACH MATERIALS => Set material state ( texture, uniform, sampler )
                            FOREACH GEOMETRY => Set object state (buffers vertex indice matrices)
                                DRAW CALL
                            END GEOMETRY
                        END MATERIALS
                    END SHADER
                END RENDER PASS
            END COMMAND BUFFER
        SUBMIT GRAPHICS COMMAND BUFFERS        
        PRESENT IMAGE        

    Graphics.Render.Dispose(Functions, Handles, PipelineData )

Graphics.Scene/Pipeline  
    Graphics.Pipeline.Load( Functions, Handles, Sce neConfig)
        convert ResourceFile into graphic pipeline data
    Graphics.Pipeline.Build/Compute( Functions, Handles, SceneConfig)
        RenderPass.CreateRenderPass(ref func , ref data, ref config);
        RenderPass.CreateFramebuffers(ref func , ref data);
        CommandBuffers.CreateCommandPool( ref func , ref data, ref config);
        CommandBuffers.CreateCommandBuffer( ref func , ref data, ref config);
        ResourceCreation.CreateVertexIndiceBuffer( ref func , ref data , ref config);
        ResourceCreation.CreateTextures( ref func , ref data , ref config);
        ResourceCreation.CreateTextureImageView
        ResourceCreation.CreateUniformBuffers( );
        ResourceCreation.CreateTextureSampler();
        ResourceDecriptor.CreateDDescriptorSetLayout(ref  func,ref  data , ref  config );
        ResourceDecriptor.CreateDDescriptorPool(ref  func,ref  data , ref  config );// HERE IS IMPORTATN FOR CREATE PIPELINE LAYOUT
        ResourceDecriptor.CreateDescriptorsSet(ref  func,ref  data , ref  config );
        ResourceDecriptor.UpdateDescriptorsSet(ref  func,ref  data , ref  config );
        ResourceDecriptor.CreatePipelineLayout( ref  func,ref  data , ref  config  );
        CreatePipeline(ref  func,ref  data , ref config.Pipeline);
            Setstates( rasterization)
    Graphics.Pipeline.Update( Functions, Handles, SceneConfig)        
        update uniform sampler, matrix ,....
    Graphics.Pipeline.Dispose( Functions, Handles, SceneConfig)